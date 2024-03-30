using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Delete(int examSlotId)
        {
            _examSlots.RemoveExamSlot(examSlotId);
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

    }
}
