using LangLang.Core.Observer;
using LangLang.Domain.Models;
using System.Collections.Generic;


namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IExamApplicationRepository
    {
        public List<ExamApplication> GetAll();
        public ExamApplication Get(int id);
        public List<ExamApplication> GetApplications(Student student);
        public List<ExamApplication> GetApplications(int examId);
        public void Add(ExamApplication app);
        public void Delete(int id);
        public void Save();
        public Dictionary<int, ExamApplication> Load();
        public void Subscribe(IObserver observer);
    }
}
