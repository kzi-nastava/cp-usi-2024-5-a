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

        public Dictionary<int, Student> GetAllStudents()
        {
            return _students.GetAllStudents();
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


        public List<Course> GetAvailableCourses(CourseController courseController) 
        {
            return _students.GetAvailableCourses(courseController);
        }
        
        public List<ExamSlot> GetAvailableExamSlots(Student student, CourseController courseController, ExamSlotController examSlotController, EnrollmentRequestController enrollmentRequestController)
        {
            return _students.GetAvailableExamSlots(student, courseController, examSlotController, enrollmentRequestController);
        }

        public bool CanModifyInfo(int studentId, EnrollmentRequestController erc)
        {
            return _students.CanModifyInfo(studentId, erc);
        }

        public List<ExamSlot> SearchExamSlotsByStudent(ExamSlotController examSlotController, CourseController courseController, EnrollmentRequestController enrollmentRequestController, int studentId, DateTime examDate, string courseLanguage, LanguageLevel? languageLevel)
        {
            List<ExamSlot> availableExamSlots = GetAvailableExamSlots(_students.GetStudentById(studentId), courseController, examSlotController, enrollmentRequestController);
            return examSlotController.SearchExamSlots(availableExamSlots, courseController, examDate, courseLanguage, languageLevel);
        }

        public List<Course> SearchCoursesByStudent(CourseController courseController, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online) 
        {
            List<Course> availableCourses = GetAvailableCourses(courseController);
            List<Course> filteredCourses = courseController.SearchCourses(availableCourses, language, level, startDate, duration, online);
            return filteredCourses;
        }
    }
}
