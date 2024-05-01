using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using System.Collections.Generic;
using LangLang.Core.Observer;
using LangLang.Core.Model.Enums;
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

        public List<Student> GetAllStudents()
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

        public void Delete(int studentId, EnrollmentRequestController erController, ExamAppRequestController earController)
        {
            _students.RemoveStudent(studentId, erController, earController);
        }

        public void Update(Student student)
        {
            _students.UpdateStudent(student);
        }

        public void Subscribe(IObserver observer)
        {
            _students.Subscribe(observer);
        }

        public List<Course> GetAvailableCourses(CourseController courseController) 
        {
            return _students.GetAvailableCourses(courseController);
        }
        
        public List<ExamSlot> GetAvailableExamSlots(Student student, CourseController courseController, ExamSlotController examSlotController, EnrollmentRequestController enrollmentRequestController)
        {
            return _students.GetAvailableExamSlots(student, courseController, examSlotController, enrollmentRequestController);
        }

        public bool CanModifyInfo(int studentId, EnrollmentRequestController erController, CourseController courseController)
        {
            return _students.CanModifyInfo(studentId, erController, courseController);
        }

        public bool CanRequestEnroll(int id, EnrollmentRequestController erController, CourseController courseController)
        {
            return _students.CanRequestEnroll(id, erController, courseController);
        }

        public List<ExamSlot> SearchExamSlotsByStudent(ExamSlotController examSlotController, CourseController courseController, EnrollmentRequestController enrollmentRequestController, int studentId, DateTime examDate, string courseLanguage, LanguageLevel? languageLevel)
        {
            Student student = GetAllStudents()[studentId];
            List<ExamSlot> availableExamSlots = GetAvailableExamSlots(student, courseController, examSlotController, enrollmentRequestController);
            return examSlotController.SearchExams(availableExamSlots, examDate, courseLanguage, languageLevel);
        }

        public List<Course> SearchCoursesByStudent(CourseController courseController, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online) 
        {
            List<Course> availableCourses = GetAvailableCourses(courseController);
            List<Course> filteredCourses = courseController.SearchCourses(availableCourses, language, level, startDate, duration, online);
            return filteredCourses;
        }

    }
}
