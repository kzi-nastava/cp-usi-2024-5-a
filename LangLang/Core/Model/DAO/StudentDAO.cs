using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository;
using LangLang.Core.Observer;
using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;

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

        public Student? RemoveStudent(Student student, AppController appController)
        {
            var enrollmentController = appController.EnrollmentRequestController;
            var examAppController = appController.ExamAppRequestController;
            var examController = appController.ExamSlotController;
            int id = student.Id;

            foreach (EnrollmentRequest er in enrollmentController.GetStudentRequests(id)) // delete all course enrollment requests
                enrollmentController.Delete(er.Id);

            foreach (ExamAppRequest ar in examAppController.GetStudentRequests(id)) // delete all exam application requests
                examAppController.Delete(ar.Id, examController);

            student.Profile.IsDeleted = true;
            _repository.Save(_students);
            NotifyObservers();
            return student;
        }


        public bool CanModifyInfo(Student student, AppController appController)
        {
            // can modify - student is not currently enrolled in any course and has not applied for any exams
            return (CanRequestEnroll(student.Id, appController) && !HasAppliedForExam(student.Id, appController));
        }

        public bool CanRequestEnroll(int id, AppController appController)
        {
            var enrollmentController = appController.EnrollmentRequestController;
            var courseController = appController.CourseController;
            var withdrawalController = appController.WithdrawalRequestController;
            
            foreach (EnrollmentRequest er in enrollmentController.GetStudentRequests(id))
            {
                if (er.Status == Status.Accepted && !er.IsCanceled)
                {
                    if (!courseController.IsCompleted(er.CourseId) && !withdrawalController.HasAcceptedWithdrawal(er.Id)) 
                        return false;
                }
            }
            return true;
        }

        public bool HasAppliedForExam(int studentId, AppController appController)
        {
            var examAppController = appController.ExamAppRequestController;
            var examController = appController.ExamSlotController;
            List<ExamAppRequest> requests = examAppController.GetActiveStudentRequests(studentId, examController);
            
            if(requests.Count == 0)
            {
                return false;
            }
            return true;
        }

    }
}
