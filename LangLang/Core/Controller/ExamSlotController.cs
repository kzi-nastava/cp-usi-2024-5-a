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

        public Dictionary<int, ExamSlot> GetAllExamSlots()
        {
            return _examSlots.GetAllExamSlots();
        }

        public bool Add(ExamSlot examSlot, CourseController courses)
        {
            if (CanCreateExamSlot(examSlot,courses))
            {
                _examSlots.AddExamSlot(examSlot);
                return true;
            }
            
            return false;
        }

        public bool Update(ExamSlot examSlot)
        {
            
            //should use const variable instead of 14

            if ((examSlot.ExamDateTime - DateTime.Now).TotalDays >= 14)
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
            ExamSlot examSlot = _examSlots.GetAllExamSlots()[examSlotId];

            //should use const variable instead of 14
            if ((examSlot.ExamDateTime - DateTime.Now).TotalDays >= 14)
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

        // Method to get all exam slots by tutor ID
        //function takes tutor id and CourseController and returns list of examslots
        public List<ExamSlot> GetExamSlotsByTutor(int tutorId, CourseController courses)
        {
            List<ExamSlot> examSlotsByTutor = new List<ExamSlot>();

            foreach (ExamSlot examSlot in _examSlots.GetAllExamSlots().Values)
            {
                // Check if the exam slot's tutor ID matches the provided tutor ID
                int courseId = examSlot.CourseId;
                int courseTutorId = courses.GetAllCourses()[courseId].TutorId;
                if (tutorId == courseTutorId)
                {
                    examSlotsByTutor.Add(examSlot);
                }
            }

            return examSlotsByTutor;
        }

        // Method to check if an exam slot is available
        // takes exam slot, returns true if it is availbale or false if it isn't available
        public bool IsExamSlotAvailable(ExamSlot examSlot)
        {
            // Check if the maximum number of students has been reached
            if (examSlot.NumberOfStudents == examSlot.MaxStudents)
            {
                return false;
            }

            // Check if there is less than a month before the exam slot starts
            TimeSpan timeUntilStart = examSlot.ExamDateTime - DateTime.Now;
            if (timeUntilStart < TimeSpan.FromDays(30))
            {
                return false;
            }

            return true;
        }

        // Method to search exam slots by tutor and criteria
        public List<ExamSlot> SearchExamSlotsByTutor(int tutorId, CourseController courses, DateTime examDate, string courseLanguage, LanguageLevel? languageLevel)
        {
            // Retrieve all exam slots created by the specified tutor
            List<ExamSlot> examSlotsByTutor = this.GetExamSlotsByTutor(tutorId, courses);
            return SearchExamSlots(examSlotsByTutor, courses, examDate, courseLanguage, languageLevel);
        }
        
        // Method to search ExamSlot list by criteria
        public List<ExamSlot> SearchExamSlots(List<ExamSlot> searchableExamSlots, CourseController courses, DateTime examDate, string courseLanguage, LanguageLevel? languageLevel)
        {
            // Apply search criteria if they are not null
            List<ExamSlot> filteredExamSlots = searchableExamSlots.Where(exam =>
                (examDate == default || exam.ExamDateTime.Date == examDate.Date) &&
                (courseLanguage == "" || courses.GetAllCourses()[exam.CourseId].Language.Contains(courseLanguage)) &&
                (languageLevel == null || courses.GetAllCourses()[exam.CourseId].Level == languageLevel)
            ).ToList();

            return filteredExamSlots;
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
    }
}
