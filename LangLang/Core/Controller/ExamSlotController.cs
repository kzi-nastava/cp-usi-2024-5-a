using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace LangLang.Core.Controller
{
    public class ExamSlotController
    {
        private readonly ExamSlotsDAO _examSlots;

        public ExamSlotController()
        {
            _examSlots = new ExamSlotsDAO();
        }

        public List<ExamSlot> GetAllExams()
        {
            return _examSlots.GetAllExams();
        }

        public bool Add(ExamSlot examSlot, CourseController courses)
        {
            if (CanCreateExamSlot(examSlot,courses))
            {
                _examSlots.AddExamSlot(examSlot);
                return true;
            }
            //delete this later
            _examSlots.AddExamSlot(examSlot);
            return false;
        }

        public bool Update(ExamSlot examSlot)
        {
            
            //should use const variable instead of 14

            if ((examSlot.TimeSlot.Time - DateTime.Now).TotalDays >= 14)
            {
                _examSlots.UpdateExamSlot(examSlot);
                return true;
            }
            else
            {
                return false;
            }
            
        }
        //returns true if removal was allowed (succesful) or false if removal wasn't allowed (unsuccesful)
        public bool Delete(int examSlotId)
        {
            ExamSlot examSlot = _examSlots.GetAllExams()[examSlotId];

            //should use const variable instead of 14
            if ((examSlot.TimeSlot.Time - DateTime.Now).TotalDays >= 14)
            {
                _examSlots.RemoveExamSlot(examSlotId);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Subscribe(IObserver observer)
        {
            _examSlots.Subscribe(observer);
        }


        public ExamSlot GetById(int id)
        {
            return _examSlots.GetAllExams()[id];
        }

        // Method to get all exam slots by tutor ID
        //function takes tutor id
        public List<ExamSlot> GetExams(Tutor tutor)
        {
            List<ExamSlot> exams = new List<ExamSlot>();

            foreach (ExamSlot exam in _examSlots.GetAllExams())
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
            // Check if the maximum number of students has been reached

            if(appController.IsFullyBooked(examSlot.Id))
            {
                return false;
            }

            // Check if there is less than a month before the exam slot starts

            TimeSpan timeUntilStart = examSlot.TimeSlot.Time - DateTime.Now;
            if (timeUntilStart < TimeSpan.FromDays(30))
            {
                return false;
            }

            return true;
        }

        // Method to search exam slots by tutor and criteria
        public List<ExamSlot> SearchExams(Tutor tutor, DateTime examDate, string  language, LanguageLevel? level)
        {
            List<ExamSlot> exams = _examSlots.GetAllExams();

            exams = this.GetExams(tutor);


            return SearchExams(exams,examDate,language,level);
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
            foreach (ExamSlot exam in _examSlots.GetAllExamSlots().Values)
            {
                DateTime examDate = exam.ExamDateTime.Date;
                Course examCourse = courses.GetAllCourses()[examSlot.CourseId];

                
                //if exams are on same day
                if(examSlot.ExamDateTime.Date == examDate)
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

            if (busyClassrooms>1)
            {
                return false;
            }
            return true;
        }

        public bool IsTimeOverlapping(TimeSpan classTime, TimeSpan examTime, int span1, int span2)
        {
            // Assuming classes and exams last for 90 minutes
            TimeSpan classEndTime = classTime.Add(TimeSpan.FromMinutes(span1));
            TimeSpan examEndTime = examTime.Add(TimeSpan.FromMinutes(span2));

            // Check for overlap
            bool overlap = !(examTime >= classEndTime || examEndTime <= classTime);
            return overlap;
        }
        /*
         public void DeleteExamSlotsByCourseId(int courseId)
        {
            List<ExamSlot> examSlotsToDelete = _examSlots.GetAllExamSlots().Values.Where(slot => slot.CourseId == courseId).ToList();
            foreach (ExamSlot slot in examSlotsToDelete)
            {
                Delete(slot.Id);
            }
        }
         */

    }
}
