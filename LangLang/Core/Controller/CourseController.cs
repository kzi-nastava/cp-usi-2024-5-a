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


        // Method checks if the course is valid for updating or canceling
        public bool IsCourseValid(int courseId)
        {
            Course course = GetAllCourses()[courseId];
            TimeSpan difference = course.StartDateTime - DateTime.Now;
            return difference.TotalDays < 7;
        }

        // Method checks if a certain course is available for the student
        public bool IsCourseAvailable(int courseId)
        {
            Course course = GetAllCourses()[courseId];
            TimeSpan difference = course.StartDateTime - DateTime.Now;
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

        public Dictionary<int, Course> GetCoursesByTutor(int tutorId)
        {
            Dictionary<int, Course> coursesByTutor = new Dictionary<int, Course>();

            foreach (Course course in GetAllCourses().Values)
            {
                if (course.TutorId == tutorId)
                {
                    coursesByTutor[course.Id] = course;
                }
            }

            return coursesByTutor;
        }

        // Method returns DateTime of the beginning of the last class of the course
        public DateTime GetCourseEnd(Course course)
        {
            DateTime end = course.StartDateTime.AddDays(7 * course.NumberOfWeeks);
            DayOfWeek endDay = course.Days[course.Days.Count - 1];
            return end.AddDays((int)endDay - (int)end.DayOfWeek);
        }

        public Course GetById(int courseId)
        {
            return _courses.GetAllCourses()[courseId];
        }
    }   
}