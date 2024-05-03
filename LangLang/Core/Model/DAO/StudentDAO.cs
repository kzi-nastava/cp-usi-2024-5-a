using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository;
using LangLang.Core.Observer;
using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using System;

namespace LangLang.Core.Model.DAO
{
    public class StudentDAO : Subject
    {
        private readonly Dictionary<int, Student> _students;
        private readonly Repository<Student> _repository;

        public StudentDAO()
        {
            _repository = new Repository<Student>("students.csv");
            _students = _repository.Load();
        }

        private int GenerateId()
        {
            if (_students.Count == 0) return 0;
            return _students.Keys.Max() + 1;
        }

        public Student? GetStudentById(int id)
        {
            return _students[id];
        }

        public List<Student> GetAllStudents()
        {
            return _students.Values.ToList();
        }

        public List<Student> GetUndeletedStudents()
        {
            List<Student> undeletedStudents = new();
            foreach (var student in GetAllStudents())
            {
                if (!student.Profile.IsDeleted) undeletedStudents.Add(student);
            }

            return undeletedStudents;
        }

        public Student AddStudent(Student student)
        {
            student.Profile.Id = GenerateId();
            _students.Add(student.Profile.Id, student);
            _repository.Save(_students);
            NotifyObservers();
            return student;
        }

        public Student? UpdateStudent(Student student)
        {
            Student? oldStudent = GetStudentById(student.Profile.Id);
            if (oldStudent == null) return null;

            oldStudent.Profile.Id = student.Profile.Id;
            oldStudent.Profile.Name = student.Profile.Name;
            oldStudent.Profile.LastName = student.Profile.LastName;
            oldStudent.Profile.Gender = student.Profile.Gender;
            oldStudent.Profile.BirthDate = student.Profile.BirthDate;
            oldStudent.Profile.PhoneNumber = student.Profile.PhoneNumber;
            oldStudent.Profile.Email = student.Profile.Email;
            oldStudent.Profile.Role = student.Profile.Role;
            oldStudent.Profile.Password = student.Profile.Password;
            oldStudent.Profession = student.Profession;

            _repository.Save(_students);
            NotifyObservers();
            return oldStudent;
        }

        public Student? RemoveStudent(int id, EnrollmentRequestController erController, ExamAppRequestController earController, ExamSlotController examSlotController)
        {
            Student? student = GetStudentById(id);
            if (student == null) return null;

            foreach (EnrollmentRequest er in erController.GetStudentRequests(id)) // delete all course enrollment requests
                erController.Delete(er.Id);

            foreach (ExamAppRequest ar in earController.GetStudentRequests(id)) // delete all exam application requests
                earController.Delete(ar.Id, examSlotController);

            _students[id].Profile.IsDeleted = true;
            _repository.Save(_students);
            NotifyObservers();
            return student;
        }

        public List<Course> GetAvailableCourses(int studentId, CourseController courseController, List<EnrollmentRequest> studentRequests)
        {
            List<Course> availableCourses = new();
            foreach (Course course in courseController.GetAllCourses().Values)
            {
                if (courseController.IsCourseAvailable(course.Id))
                {
                    if (!IsRequestDuplicate(studentId, course, studentRequests))
                        availableCourses.Add(course);
                }
            }
            return availableCourses;
        }

        private bool IsRequestDuplicate(int studentId, Course course, List<EnrollmentRequest> enrollmentRequests)
        {
            foreach (EnrollmentRequest er in enrollmentRequests)
            {
                if (er.StudentId == studentId && er.CourseId == course.Id && !er.IsCanceled) return true;
            }
            return false;
        }

        private bool HasStudentAttendedCourse(Course course, EnrollmentRequest enrollmentRequest, ExamSlot examSlot)
        {

            if (course.Language == examSlot.Language && course.Level == examSlot.Level)
            {
                if (enrollmentRequest.Status == Status.Accepted && course.IsCompleted())
                {
                    return true;
                }
            }
            return false;
        }

        public List<ExamSlot>? GetAvailableExamSlots(Student student, CourseController courseController, ExamSlotController examSlotController, EnrollmentRequestController erController)
        {
            if (student == null) return null;
            List<ExamSlot> availableExamSlots = new();

            List<EnrollmentRequest> studentRequests = erController.GetStudentRequests(student.Id);
            
            foreach (ExamSlot examSlot in examSlotController.GetAllExams())
            {
                foreach (EnrollmentRequest enrollmentRequest in studentRequests)
                {
                    Course course = courseController.GetById(enrollmentRequest.CourseId);
                    if (HasStudentAttendedCourse(course, enrollmentRequest, examSlot))
                    {
                        availableExamSlots.Add(examSlot);
                    }
                }
            }

            return availableExamSlots;
        }

        public bool CanModifyInfo(int studentId, EnrollmentRequestController erController, CourseController courseController, WithdrawalRequestController wrController)
        {
            // can modify - student is not currently enrolled in any course and has not applied for any exams
            return (CanRequestEnroll(studentId, erController, courseController, wrController) && !HasRegisteredForExam());
        }

        public bool CanRequestEnroll(int id, EnrollmentRequestController erController, CourseController courseController, WithdrawalRequestController wrController)
        {
            foreach (EnrollmentRequest er in erController.GetStudentRequests(id))
            {
                if (er.Status == Status.Accepted && !er.IsCanceled)
                {
                    if (!courseController.IsCompleted(er.CourseId) && !wrController.HasAcceptedWithdrawal(er.Id)) 
                        return false;
                }
            }
            return true;
        }

        public bool HasRegisteredForExam()
        {
            // TODO: Implement this method once the exam application class is implemented.
            return false;
        }
    }
}
