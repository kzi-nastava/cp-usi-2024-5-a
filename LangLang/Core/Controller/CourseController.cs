using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Model.Enums;
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

        public Dictionary<int, Course> GetAll()
        {
            return _courses.GetAll();
        }

        public void Add(Course course)
        {
            _courses.Add(course);
        }

        public void Update(Course course)
        {
            _courses.Update(course);
        }

        public void Delete(int courseId)
        {
            _courses.Remove(courseId);
        }

        // Deletes all future courses made by tutor or updates all the active courses to have no tutor as well as future courses made by director
        public void DeleteByTutor(Tutor tutor)
        {
            _courses.DeleteByTutor(tutor);
        }

        public void Subscribe(IObserver observer)
        {
            _courses.Subscribe(observer);
        }

        // Method checks if the course is valid for updating or canceling
        public bool CanChange(int courseId)
        {
            return _courses.CanChange(courseId);
        }

        // Method checks if a certain course is available for the student
        public bool IsAvailable(int courseId)
        {
            return _courses.IsAvailable(courseId);
        }

        public void AddStudent(int courseId)
        {
            _courses.AddStudent(courseId);
        }

        public void RemoveStudent(int courseId)
        {
            _courses.RemoveStudent(courseId);
        }

        public List<Course> GetByTutor(Tutor tutor)
        {
            return _courses.GetByTutor(tutor);
        }

        public List<Course> GetByTutor(int tutorId)
        {
            return _courses.GetByTutor(tutorId);
        }

        public DateTime GetEnd(Course course)
        {
            return _courses.GetEnd(course);
        }

        public Course Get(int courseId)
        {
            return _courses.GetAll()[courseId];
        }

        public List<Course> SearchCoursesByTutor(int tutorId, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online)
        {
            return _courses.SearchCoursesByTutor(tutorId, language, level, startDate, duration, online);           
        }

        public List<Course> SearchCoursesByStudent(AppController appController, Student student, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online)
        {
            return _courses.SearchCoursesByStudent(appController, student, language, level, startDate, duration, online);
        }

        public bool IsCompleted(int id)
        {
            return _courses.IsCompleted(id);
        }

        public bool IsActive(Course course)
        {
            return _courses.IsActive(course);
        }

        public bool OverlappsWith(Course course, TimeSlot timeSlot)
        {
            return course.OverlappsWith(timeSlot);
        }

        public bool CanCreateOrUpdate(Course course, ExamSlotController examSlotController)
        {
            return _courses.CanCreateOrUpdate(course, examSlotController);
        }
      
        public List<Course> GetBySkills(Tutor tutor)
        {
            return _courses.GetBySkills(tutor);
        }

        public List<Course> GetCompleted(Student student, AppController appController)
        {
            return _courses.GetCompleted(student, appController);
        }

        public List<Course> GetAvailable(Student student, AppController appController)
        {
            return _courses.GetAvailable(student, appController);
        }

    }   

}