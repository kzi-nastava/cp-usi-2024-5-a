using LangLang.Domain.Models;
using System.Collections.Generic;

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
        /*
        public List<ExamSlot> Search(List<ExamSlot> exams, DateTime examDate, string language, LanguageLevel? level);
        public List<ExamSlot> SearchByTutor(Tutor tutor, DateTime examDate, string language, LanguageLevel? level);
        public List<ExamSlot> SearchByStudent(AppController appController, Student student, DateTime examDate, string courseLanguage, LanguageLevel? languageLevel);
        */
    }
}
