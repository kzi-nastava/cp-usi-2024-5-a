using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using System.Collections.Generic;
using LangLang.Core.Observer;
using LangLang.Core.Model.Enums;

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

        public Dictionary<int, Course>? GetAvailableCourses(CourseController courseController) 
        {
            Dictionary<int, Course> availableCourses = new();
            foreach (Course course in courseController.GetAllCourses().Values)
            {
                if (courseController.IsCourseAvailable(course.Id)) {
                        availableCourses.Add(course.Id, course);
                }
            }
            return availableCourses;
        }

        private bool HasStudentAttendedCourse(Student student, Course course, EnrollmentRequest enrollmentRequest)
        {
   
            if (enrollmentRequest.StudentId == student.Id && enrollmentRequest.CourseId == course.Id)
            {
                if (enrollmentRequest.ERStatus == ERStatus.Accepted)
                {
                    return true;
                }
            }
            return false;
        }

        public Dictionary<int, ExamSlot>? GetAvailableExamSlots(Student student, CourseController courseController, ExamSlotController examSlotController, EnrollmentRequestController enrollmentRequestController)
        {
            Dictionary<int, ExamSlot> availableExamSlots = new();
            foreach (ExamSlot examSlot in examSlotController.GetAllExamSlots().Values)
            {
                foreach (EnrollmentRequest enrollmentRequest in enrollmentRequestController.GetStudentRequests(student.Id).Values) {
                    if (HasStudentAttendedCourse(student, courseController.GetById(examSlot.CourseId), enrollmentRequest)) {
                        availableExamSlots.Add(examSlot.Id, examSlot);
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
    }
}
