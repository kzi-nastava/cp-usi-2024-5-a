using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.WPF.Views.TutorView.Tabs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories.SqlRepositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DatabaseContext _databaseContext;

        public CourseRepository(DatabaseContext context)
        {
            _databaseContext = context;
        }

        public Course Get(int id)
        {
            return _databaseContext.Course.Find(id);
        }

        public List<Course> GetAll()
        {
            return _databaseContext.Course.ToList();
        }

        public void Add(Course course)
        {
            _databaseContext.Course.Add(course);
            _databaseContext.SaveChanges();
        }

        public void Update(Course course)
        {
            var existingCourse = _databaseContext.Course.Find(course.Id);
            if (existingCourse != null)
            {
                _databaseContext.Entry(existingCourse).State = EntityState.Detached;
            }

            _databaseContext.Course.Update(course);
            _databaseContext.SaveChanges();
        }


        public void Delete(Course course)
        {
            var existingCourse = _databaseContext.Course.Find(course.Id);
            if (existingCourse != null)
            {
                _databaseContext.Course.Remove(existingCourse);
                _databaseContext.SaveChanges();
            }
        }
        public List<Course> GetWithoutTutor()
        {
            List<Course> courses = GetAll();
            List<Course> res = new();
            foreach ( var course in courses)
            {
                if(course.TutorId == Constants.DELETED_TUTOR_ID)
                {
                    res.Add(course);
                }
            }
            return res;
        }


    }
}
