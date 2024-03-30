using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Observer;
using System.Collections.Generic;

namespace LangLang.Core.Controller
{
    public class CourseController
    {
        private readonly CoursesDAO _courses;

        public CourseController()
        {
            _courses = new CoursesDAO();
        }

        public Dictionary<Course> GetAllCourses()
        {
            return _courses.GetAllCourses();
        }

        public void Add(Course course)
        {
            _courses.AddCourse(course);
        }

        public void Delete(int courseId)
        {
            _courses.RemoveCourse(courseId);
        }

        public void Subscribe(IObserver observer)
        {
            _courses.Subscribe(observer);
        }

    }   
}