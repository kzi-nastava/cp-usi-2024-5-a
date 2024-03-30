using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Observer;
using System;
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

        public Dictionary<int, Course> GetAllCourses()
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

        // Method checks if a certain course is available for the student
        public bool isCourseAvailable(int id)
        {
            Course course = GetAllCourses()[id];
            TimeSpan difference = course.StartDate - DateTime.Now;
            if (difference.TotalDays < 7)
            {
                if (course.Online)
                {
                    return true;
                }
                else
                {
                    return course.NumberOfStudents <= course.MaxStudents;
                }
            }
            else
            {
                return false;
            }
        }

        // Returns all courses made by a certain tutor
        public Dictionary<int, Course> GetCoursesByTutor(Tutor tutor)
        {
            Dictionary<int, Course> coursesByTutor = new Dictionary<int, Course>();

            foreach (Course course in GetAllCourses().Values)
            {
                if (course.TutorId == tutor.Id)
                {
                    coursesByTutor[course.Id] = course;
                }
            }

            return coursesByTutor;
        }

    }   
}