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

        public bool OverlappsWith(Course course, TimeSlot timeSlot)
        {
            return course.OverlappsWith(timeSlot);
        }

        public bool CanCreateOrUpdateCourse(Course course, ExamSlotController examSlotController)
        {
            return _courses.CanCreateOrUpdateCourse(course, examSlotController);
        }
      
        public List<Course> GetCoursesForSkills(Tutor tutor)
        {
            return _courses.GetCoursesForSkills(tutor);
        }

        public List<Course> GetCompletedCourses(int studentId, AppController appController)
        {
            var enrollmentController = appController.EnrollmentRequestController;
            var withdrawalController = appController.WithdrawalRequestController;
            var studentRequests = enrollmentController.GetStudentRequests(studentId);
            List<Course> courses = new();

            foreach (var request in studentRequests)
            {
                Course course = GetById(request.CourseId);
                if (StudentAttendedUntilEnd(course, request, withdrawalController))
                    courses.Add(course);
            }
            return courses;
        }

        private bool StudentAttendedUntilEnd(Course course, EnrollmentRequest request, WithdrawalRequestController wrController)
        {
            return course.IsCompleted() && request.Status == Status.Accepted 
                    && !wrController.HasAcceptedWithdrawal(request.Id);
        }

        public List<Course> GetAvailableCourses(Student student, AppController appController)
        {
            return _courses.GetAvailableCourses(student, appController);
        }

    }   

}