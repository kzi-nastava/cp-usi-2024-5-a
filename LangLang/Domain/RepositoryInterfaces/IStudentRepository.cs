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
        public void Deactivate(int id);
        public void Save();
        public void Load();
    }
}
