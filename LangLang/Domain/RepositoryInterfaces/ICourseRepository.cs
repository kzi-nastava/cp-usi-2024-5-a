using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Domain.RepositoryInterfaces
{
    internal interface ICourseRepository
    {
        public List<Course> GetAll();
        public Course Get(int id);
        public List<Course> GetWithoutTutor();
        public void Add(Course course);
        public void Update(Course course);
        public void Delete(Course course);
    }
}
