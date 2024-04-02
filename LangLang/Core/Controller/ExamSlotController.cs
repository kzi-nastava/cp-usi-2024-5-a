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

        public void Add(ExamSlot examSlot)
        {
            _examSlots.AddExamSlot(examSlot);
        }
        /*
        public void Delete(int examSlotId)
        {
            _examSlots.RemoveExamSlot(examSlotId);
        }
        */

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
        public List<ExamSlot> SearchExamSlotsByTutor(int tutorId, CourseController courses, DateTime examDate, string courseLanguage, LanguageLevel languageLevel)
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
                (courseLanguage == "" || courses.GetAllCourses()[exam.CourseId].Language == courseLanguage) &&
                (languageLevel == null || courses.GetAllCourses()[exam.CourseId].Level == languageLevel)
            ).ToList();

            return filteredExamSlots;
        }

    }
}
