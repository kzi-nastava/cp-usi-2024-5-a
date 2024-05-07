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

        public Student? Get(int id)
        {
            return _students[id];
        }

        public List<Student> GetAll()
        {
            return _students.Values.ToList();
        }



        public Student Add(Student student)
        {
            student.Profile.Id = GenerateId();
            _students.Add(student.Profile.Id, student);
            _repository.Save(_students);
            NotifyObservers();
            return student;
        }

        public Student? Update(Student student)
        {
            Student? oldStudent = Get(student.Profile.Id);
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

        public void Deactivate(int id, AppController appController)
        {
            Student student = Get(id);
            if (student == null) return;

            var enrollmentController = appController.EnrollmentRequestController;
            var examAppController = appController.ExamAppRequestController;
            var examController = appController.ExamSlotController;
            
            foreach (EnrollmentRequest er in enrollmentController.GetRequests(student)) // delete all course enrollment requests
                enrollmentController.Delete(er.Id);

            foreach (ExamAppRequest ar in examAppController.GetRequests(student)) // delete all exam application requests
                examAppController.Delete(ar.Id, examController);

            _students[id].Profile.IsActive = true;
            _repository.Save(_students);
            NotifyObservers();
        }


        public bool CanModifyData(Student student, AppController appController)
        {
            // can modify - student is not currently enrolled in any course and has not applied for any exams
            return (CanRequestEnrollment(student, appController) && !HasAppliedForExam(student.Id, appController));
        }

        public bool CanRequestEnrollment(Student student, AppController appController)
        {
            var enrollmentController = appController.EnrollmentRequestController;
            var courseController = appController.CourseController;
            var withdrawalController = appController.WithdrawalRequestController;
            
            foreach (EnrollmentRequest er in enrollmentController.GetRequests(student))
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

            return requests.Count != 0;
        }
        public bool CanApplyForCourses(Student student, AppController appController)
        {
            bool hasNoResults = appController.ExamAppRequestController.HasNoGeneratedResults(student, appController.ExamSlotController);
            bool hasNotGradedExams = appController.ExamResultController.HasNotGradedResults(student);
            return !hasNoResults && !hasNotGradedExams;
        }

        public bool CanApplyForExams(Student student, AppController appController)
        {
            if (CanApplyForCourses(student, appController)) {
                bool hasPreliminaryResults = appController.ExamResultController.HasPreliminaryResults(student);
                return !hasPreliminaryResults;
            }
            return false;
        }
    }
}
