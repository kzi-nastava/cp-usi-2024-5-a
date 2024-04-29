using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Core.Observer;
using System.Collections;
using System.Windows.Input;
using LangLang.Core.Controller;

namespace LangLang.Core.Model.DAO
{
    public class ExamSlotsDAO : Subject
    {
        private readonly Dictionary<int, ExamSlot> _exams;
        private readonly Repository<ExamSlot> _repository;

        public ExamSlotsDAO()
        {
            _repository = new Repository<ExamSlot>("examSlots.csv");
            _exams = _repository.Load();
        }
        private int GenerateId()
        {
            if (_exams.Count == 0) return 0;
            return _exams.Keys.Max() + 1;
        }

        public ExamSlot? GetExamById(int id)
        {
            return _exams[id];
        }

        public Dictionary<int, ExamSlot> GetAllExams()
        {
            return _exams;
        }


        //function takes examslot and adds it to dictionary of examslots
        //function saves changes and returns if adding was successful
        public bool AddExam(ExamSlot exam, CourseController courses)
        {
            if (CanCreateExamSlot(exam, courses))
            {
                exam.Id = GenerateId();
                _exams[exam.Id] = exam;
                _repository.Save(_exams);
                NotifyObservers();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        //function takes id of examslot and removes examslot with that id
        //function saves changes and returns if removing was successful
        public bool RemoveExam(int id)
        {
            ExamSlot exam = GetExamById(id);
            if (exam == null) return false;

            //should use const variable instead of 14
            if ((exam.TimeSlot.Time - DateTime.Now).TotalDays >= 14)
            {
                _exams.Remove(id);
                _repository.Save(_exams);
                NotifyObservers();
                return true;
            }
            else
            {
                return false;
            }

        }

        //function for updating examslot takes new version of examslot and updates existing examslot to be same as new one
        //function saves changes and returns if updating was successful
        public bool UpdateExam(ExamSlot exam)
        {
            //should use const variable instead of 14

            if ((exam.TimeSlot.Time - DateTime.Now).TotalDays >= 14)
            {
                ExamSlot oldExam = GetExamById(exam.Id);
                if (oldExam == null) return false;

                oldExam.TutorId = exam.TutorId;
                oldExam.MaxStudents = exam.MaxStudents;
                oldExam.TimeSlot = exam.TimeSlot;

                _repository.Save(_exams);
                NotifyObservers();
                return true;
            }
            else
            {
                return false;
            }
            
        }


        // Method to get all exam slots by tutor ID
        //function takes tutor id
        public List<ExamSlot> GetExams(Tutor tutor)
        {
            List<ExamSlot> exams = new List<ExamSlot>();

            foreach (ExamSlot exam in _exams.Values)
            {

                if (tutor.Id == exam.TutorId)
                {
                    exams.Add(exam);
                }
            }

            return exams;
        }


        // Method to check if an exam slot is available
        // takes exam slot, returns true if it is availbale or false if it isn't available
        public bool IsAvailable(ExamSlot examSlot, ExamAppRequestController appController)
        {
            // Check if exam passed
            if(examSlot.TimeSlot.Time > DateTime.Now)
            {
                return false;
            }

            // Check if the maximum number of students has been reached

            if (appController.IsFullyBooked(examSlot.Id))
            {
                return false;
            }

            return true;
        }


        // Method to search exam slots by tutor and criteria
        public List<ExamSlot> SearchExamsByTutor(Tutor tutor, DateTime examDate, string language, LanguageLevel? level)
        {
            List<ExamSlot> exams = _exams.Values.ToList();

            exams = this.GetExams(tutor);

            return SearchExams(exams, examDate, language, level);
        }
        public List<ExamSlot> SearchExams(List<ExamSlot> exams, DateTime examDate, string language, LanguageLevel? level)
        {

            // Apply search criteria if they are not null
            List<ExamSlot> filteredExams = exams.Where(exam =>
                (examDate == default || exam.TimeSlot.Time.Date == examDate.Date) &&
                (language == "" || exam.Language == language) &&
                (level == null || exam.Level == level)
            ).ToList();

            return filteredExams;
        }







        public bool CanCreateExamSlot(ExamSlot examSlot, CourseController courses)
        {

            //course of exam slot
            Course course = courses.GetAllCourses()[examSlot.CourseId];

            TimeSpan courseTime = course.StartDateTime.TimeOfDay;
            DateTime courseStartDate = course.StartDateTime.Date;
            DateTime courseEndDate = courses.GetCourseEnd(course);

            int busyClassrooms = 0;

            //if course isn't finished, can't create exam slot
            if (courseEndDate >= examSlot.ExamDateTime.Date)
            {
                return false;
            }

            // Go through each course with the same tutor and check if there is class overlapping with exam 
            foreach (Course cour in courses.GetAllCourses().Values)
            {
                // Check if the course and exam are overlapping
                DateTime courStartDate = cour.StartDateTime.Date;
                DateTime courEndDate = courses.GetCourseEnd(cour);

                // Calculate class dates based on course start date, end date, weekdays, and time
                List<DateTime> classDates = courses.CalculateClassDates(courStartDate, courEndDate, cour.Days, cour.StartDateTime.TimeOfDay);

                // Check if exam date overlaps with any class date
                foreach (var classDate in classDates)
                {
                    if (classDate.Date == examSlot.ExamDateTime.Date)
                    {
                        if (IsTimeOverlapping(classDate.TimeOfDay, examSlot.ExamDateTime.TimeOfDay, 90, 360))
                        {
                            //tutor is busy
                            if (cour.TutorId == course.TutorId)
                            {
                                return false;
                            }
                            //classroom is busy
                            else
                            {
                                if (!cour.Online)
                                {
                                    busyClassrooms += 1;
                                }
                            }

                        }
                    }
                }
            }

            // Go through all exams of the same tutor
            foreach (ExamSlot exam in _exams.GetAllExamSlots().Values)
            {
                DateTime examDate = exam.ExamDateTime.Date;
                Course examCourse = courses.GetAllCourses()[examSlot.CourseId];


                //if exams are on same day
                if (examSlot.ExamDateTime.Date == examDate)
                {
                    //checks if they are overlapping
                    if (IsTimeOverlapping(exam.ExamDateTime.TimeOfDay, examSlot.ExamDateTime.TimeOfDay, 360, 360))
                    {
                        if (course.TutorId == examCourse.TutorId)
                        {
                            return false;
                        }
                        else
                        {
                            busyClassrooms += 1;
                        }
                    }

                }

            }

            if (busyClassrooms > 1)
            {
                return false;
            }
            return true;
        }
    }
    
}
