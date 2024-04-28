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
            List<Course> availableCourses = new();
            foreach (Course course in courseController.GetAllCourses().Values)
            {
                if (courseController.IsCourseAvailable(course.Id)) {
                        availableCourses.Add(course);
                }
            }
            return availableCourses;
        }

        private bool HasStudentAttendedCourse(Student student, Course course, EnrollmentRequest enrollmentRequest)
        {
   
            if (enrollmentRequest.StudentId == student.Id && enrollmentRequest.CourseId == course.Id)
            {
                if (enrollmentRequest.Status == Status.Accepted)
                {
                    return true;
                }
            }
            return false;
        }

        public List<ExamSlot> GetAvailableExamSlots(Student student, CourseController courseController, ExamSlotController examSlotController, EnrollmentRequestController enrollmentRequestController)
        {
            List<ExamSlot> availableExamSlots = new();
            if (student == null) return availableExamSlots; 
            foreach (ExamSlot examSlot in examSlotController.GetAllExamSlots().Values)
            {
                foreach (EnrollmentRequest enrollmentRequest in enrollmentRequestController.GetStudentRequests(student.Id).Values) {
                    if (HasStudentAttendedCourse(student, courseController.GetById(examSlot.CourseId), enrollmentRequest)) {
                        availableExamSlots.Add(examSlot);
                    }
                }
            }

            return availableExamSlots;
        }

        public bool CanModifyInfo(int studentId, EnrollmentRequestController erc)
        {
            Dictionary<int, EnrollmentRequest> studentRequests = erc.GetStudentRequests(studentId);
            return studentRequests.Count == 0;
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
