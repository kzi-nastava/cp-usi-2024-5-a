using LangLang.Core.Model;
using LangLang.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Models;
using LangLang.BusinessLogic.UseCases;

namespace LangLang.Core.Controller
{
    public class ExamSlotController
    {
        private readonly ExamSlotService _exams;

        public ExamSlotController()
        {
            _exams = new ExamSlotService();
        }

        public List<ExamSlot> GetAll()
        {
            return _exams.GetAll().Values.ToList();
        }

        public bool Add(ExamSlot exam, CourseController courses)
        {
            return _exams.Add(exam, courses);
        }
        public bool CanCreateExam(ExamSlot exam, CourseController courseController)
        {
            return _exams.CanCreateExam(exam, courseController);
        }
        public void Update(ExamSlot exam)
        {
            _exams.Update(exam);
        }
        public bool CanBeUpdated(ExamSlot exam)
        {
            return _exams.CanBeUpdated(exam);
        }
        public bool Delete(int examId)
        {
            return _exams.Delete(examId);
        }

        public bool ApplicationsVisible(int id)
        {
            return _exams.ApplicationsVisible(id);
        }

        public void Subscribe(IObserver observer)
        {
            _exams.Subscribe(observer);
        }

        public ExamSlot? Get(int id)
        {
            return _exams.Get(id);
        }

        public List<ExamSlot> GetExams(Tutor tutor)
        {
            return _exams.GetExams(tutor);
        }
        public List<ExamSlot> SearchByTutor(Tutor tutor, DateTime examDate, string language, LanguageLevel? level)
        {
            return _exams.SearchByTutor(tutor, examDate, language, level);
        }

        public List<ExamSlot> SearchByStudent(AppController appController, Student student, DateTime examDate, string language, LanguageLevel? level)
        {
            return _exams.SearchByStudent(appController, student, examDate, language, level);
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
