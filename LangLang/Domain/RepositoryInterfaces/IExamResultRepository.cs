using LangLang.Core.Model;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;


namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IExamResultRepository
    {
        public List<ExamResult> GetAll();
        public ExamResult Get(int id);
        public List<ExamResult> GetByExam(ExamSlot exam);
        public List<ExamResult> GetByStudent(Student student);
        public void Add(ExamResult examResult);
        public void Update(ExamResult examResult);
        public bool HasNotGradedResults(Student student);
        public bool HasPreliminaryResults(Student student);
        public void Save();
        public Dictionary<int, ExamResult> Load();
    }
}
