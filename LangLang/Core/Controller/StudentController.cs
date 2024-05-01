using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using System.Collections.Generic;
using LangLang.Core.Observer;
using System;

namespace LangLang.Core.Controller
{
    public class StudentController
    {
        private readonly StudentDAO _students;

        public StudentController()
        {
            _students = new StudentDAO();
        }

        public Dictionary<int, Student> GetAllStudents()
        {
            return _students.GetAllStudents();
        }

        public Student GetById(int id)
        {
            return _students.GetStudentById(id);
        }

        public void Add(Student student)
        {
            _students.AddStudent(student);
        }

        public void Delete(int studentId, EnrollmentRequestController enrollmentRequestController)
        {
            _students.RemoveStudent(studentId, enrollmentRequestController);
        }

        public void Update(Student student)
        {
            _students.UpdateStudent(student);
        }

        public void Subscribe(IObserver observer)
        {
            _students.Subscribe(observer);
        }

        public List<Course> GetAvailableCourses(int studentId, CourseController courseController, EnrollmentRequestController erController) 
        {
            List<EnrollmentRequest> studentRequests = erController.GetStudentRequests(studentId);
            return _students.GetAvailableCourses(studentId, courseController, studentRequests);
        }
        
        public List<ExamSlot> GetAvailableExamSlots(Student student, CourseController courseController, ExamSlotController examSlotController, EnrollmentRequestController enrollmentRequestController)
        {
            return _students.GetAvailableExamSlots(student, courseController, examSlotController, enrollmentRequestController);
        }

        public bool CanModifyInfo(int studentId, EnrollmentRequestController erController, CourseController courseController, WithdrawalRequestController wrController)
        {
            return _students.CanModifyInfo(studentId, erController, courseController, wrController);
        }

        public bool CanRequestEnroll(int id, EnrollmentRequestController erController, CourseController courseController, WithdrawalRequestController wrController)
        {
            return _students.CanRequestEnroll(id, erController, courseController, wrController);
        }

        public List<ExamSlot> SearchExamSlotsByStudent(ExamSlotController examSlotController, CourseController courseController, EnrollmentRequestController enrollmentRequestController, int studentId, DateTime examDate, string courseLanguage, LanguageLevel? languageLevel)
        {
            Student student = GetAllStudents()[studentId];
            List<ExamSlot> availableExamSlots = GetAvailableExamSlots(student, courseController, examSlotController, enrollmentRequestController);
            return examSlotController.SearchExams(availableExamSlots, examDate, courseLanguage, languageLevel);
        }

        public List<Course> SearchCoursesByStudent(int studentId, CourseController courseController, EnrollmentRequestController erController, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online) 
        {
            List<Course> availableCourses = GetAvailableCourses(studentId, courseController, erController);
            List<Course> filteredCourses = courseController.SearchCourses(availableCourses, language, level, startDate, duration, online);
            return filteredCourses;
        }

    }
}
