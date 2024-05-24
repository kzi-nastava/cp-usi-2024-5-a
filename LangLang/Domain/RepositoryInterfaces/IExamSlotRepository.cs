
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IExamSlotRepository
    {
        public List<ExamSlot> GetAll();
        public ExamSlot Get(int id);
        public List<ExamSlot> GetExams(Tutor tutor);
        public void Add(ExamSlot exam);
        public void Delete(int id);
        public void Update(ExamSlot exam);
        public void Save();
        public Dictionary<int, ExamSlot> Load();
        public void Subscribe(IObserver observer);
        /*
        public List<ExamSlot> Search(List<ExamSlot> exams, DateTime examDate, string language, LanguageLevel? level);
        public List<ExamSlot> SearchByTutor(Tutor tutor, DateTime examDate, string language, LanguageLevel? level);
        public List<ExamSlot> SearchByStudent(AppController appController, Student student, DateTime examDate, string courseLanguage, LanguageLevel? languageLevel);
        */
    }
}
