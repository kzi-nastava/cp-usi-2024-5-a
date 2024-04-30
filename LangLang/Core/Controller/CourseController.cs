using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Dictionary<int, Course> GetLiveCourses()
        {
            return _courses.GetLiveCourses();
        }

        public void Add(Course course)
        {
            _courses.AddCourse(course);
        }

        public void Update(Course course)
        {
            _courses.UpdateCourse(course);
        }

        public void Delete(int courseId)
        {
            _courses.RemoveCourse(courseId);
        }

        // Deletes all future courses made by tutor or updates all the active courses to have no tutor as well as future courses made by director
        public void DeleteCoursesWithTutor(int tutorId)
        {
            _courses.DeleteCoursesWithTutor(tutorId);
        }

        public void Subscribe(IObserver observer)
        {
            _courses.Subscribe(observer);
        }

        // Method checks if the course is valid for updating or canceling
        public bool CanCourseBeChanged(int courseId)
        {
            return _courses.CanCourseBeChanged(courseId);
        }

        // Method checks if a certain course is available for the student
        public bool IsCourseAvailable(int courseId)
        {
            return _courses.IsCourseAvailable(courseId);
        }

        public void AddStudentToCourse(int courseId)
        {
            _courses.AddStudentToCourse(courseId);
        }

        public Dictionary<int, Course> GetCoursesWithTutor(Tutor tutor)
        {
            return _courses.GetCoursesWithTutor(tutor);
        }

        public Dictionary<int, Course> GetCoursesWithTutor(int tutorId)
        {
            return _courses.GetCoursesWithTutor(tutorId);
        }
        public List<Course> GetCourses(Tutor tutor)
        {
            return _courses.GetCoursesWithTutor(tutor).Values.ToList();
        }

        public DateTime GetCourseEnd(Course course)
        {
            return _courses.GetCourseEnd(course);
        }

        public Course GetById(int courseId)
        {
            return _courses.GetAllCourses()[courseId];
        }

        public List<Course> SearchCoursesByTutor(int tutorId, string language, LanguageLevel level, DateTime startDate, int duration, bool online)
        {
            List<Course> tutorsCourses = GetCoursesWithTutor(tutorId).Values.ToList();
            return SearchCourses(tutorsCourses, language, level, startDate, duration, online);
        }

        public List<Course> SearchCourses(List<Course> searchableCourses, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online)
        {
            List<Course> filteredCourses = searchableCourses.Where(course =>
            (language == "" || course.Language.Contains(language)) && 
            (level == null || course.Level == level) &&
            (startDate == default || course.StartDateTime.Date == startDate.Date) &&
            (duration == 0 || course.NumberOfWeeks == duration) &&
            (online == false || course.Online == online)).ToList();

            return filteredCourses;
        }

        public bool IsCompleted(int id)
        {
            return _courses.IsCompleted(id);
        }
    }   
}