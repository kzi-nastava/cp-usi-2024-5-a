using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Update(ExamSlot exam)
        {
            _exams.UpdateExam(exam);
        }
        public bool CanBeUpdated(ExamSlot exam)
        {
            return _exams.CanBeUpdated(exam);
        }
        public bool Delete(int examId)
        {
            return _exams.RemoveExam(examId);
        }

        public bool ApplicationsVisible(int id)
        {
            return _exams.ApplicationsVisible(id);
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

        public List<ExamSlot> SearchExamsByStudent(AppController appController, Student student, DateTime examDate, string language, LanguageLevel? level)
        {
            return _exams.SearchExamsByStudent(appController, student, examDate, language, level);
        }

        public bool HasPassed(ExamSlot exam)
        {
            return _exams.HasPassed(exam);
        }
       
        public void AddStudent(ExamSlot exam)
        {
            _exams.AddStudent(exam);
        }

        public void RemoveStudent(ExamSlot exam)
        {
            _exams.RemoveStudent(exam);
        }

        // returns a list of exams that are available for student application
        public List<ExamSlot>? GetAvailableExams(Student student, AppController appController)
        {
            return _exams.GetAvailableExams(student, appController);    
        }

    }
}
