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
        private readonly ExamSlotsDAO _exams;

        public ExamSlotController()
        {
            _exams = new ExamSlotsDAO();
        }

        public List<ExamSlot> GetAllExams()
        {
            return _exams.GetAllExams().Values.ToList();
        }

        public bool Add(ExamSlot exam, CourseController courses)
        {
            return _exams.AddExam(exam, courses);
        }

        public bool Update(ExamSlot exam)
        {
            return _exams.UpdateExam(exam);
        }

        public bool Delete(int examId)
        {
            return _exams.RemoveExam(examId);
        }

        public void Subscribe(IObserver observer)
        {
            _exams.Subscribe(observer);
        }

        public ExamSlot? GetById(int id)
        {
            return _exams.GetExamById(id);
        }

        public List<ExamSlot> GetExams(Tutor tutor)
        {
            return _exams.GetExams(tutor);
        }
        public List<ExamSlot> SearchExamsByTutor(Tutor tutor, DateTime examDate, string language, LanguageLevel? level)
        {
            return _exams.SearchExamsByTutor(tutor, examDate, language, level);
        }

        public List<ExamSlot> SearchExams(List<ExamSlot> exams, DateTime examDate, string language, LanguageLevel? level)
        {
            return _exams.SearchExams(exams, examDate, language, level);
        }
    }
}
