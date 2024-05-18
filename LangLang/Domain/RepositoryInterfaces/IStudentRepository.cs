
using LangLang.Core.Controller;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IStudentRepository
    {
        public List<Student> GetAll();
        public Student Get(int id);
        public void Add(Student student);
        public void Update(Student student);
        public void Deactivate(int id, AppController appController); // TODO: delete second param
        public void Save();
        public Dictionary<int, Student> Load();
        public void Subscribe(IObserver observer);
    }
}
