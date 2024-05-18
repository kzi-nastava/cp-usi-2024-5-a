using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using System.Collections.Generic;
using LangLang.Domain.RepositoryInterfaces;
using System.Linq;
using LangLang.Core.Observer;
using LangLang.Composition;

namespace LangLang.Aplication.UseCases
{
    public class StudentService
    {
        // TODO: Remove the appController parameter when applying DIP in whole project
     
        private IStudentRepository _students {  get; set; }

        public StudentService() 
        { 
            _students = Injector.CreateInstance<IStudentRepository>();
        }

        private int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public List<Student> GetAll()
        {
            return _students.GetAll();
        }

        public Student Get(int id)
        {
            return _students.Get(id);
        }

        public void Add(Student student)
        {
            student.Profile.Id = GenerateId();
            _students.Add(student);
        }
        public void Update(Student student)
        {
            _students.Update(student);
        }

        public void Deactivate(int id, AppController appController) 
        {
            _students.Deactivate(id, appController);
        }

        public void Subscribe(IObserver observer)
        {
            _students.Subscribe(observer);
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
            var examAppController = appController.ExamApplicationController;
            var examController = appController.ExamSlotController;
            List<ExamApplication> requests = examAppController.GetActiveStudentApplications(studentId, examController);

            return requests.Count != 0;
        }

        public bool CanApplyForCourses(Student student, AppController appController)
        {
            bool hasNoResults = appController.ExamApplicationController.HasNoGeneratedResults(student, appController.ExamSlotController);
            bool hasNotGradedExams = appController.ExamResultController.HasNotGradedResults(student);
            return !hasNoResults && !hasNotGradedExams;
        }

        public bool CanApplyForExams(Student student, AppController appController)
        {
            if (CanApplyForCourses(student, appController))
            {
                bool hasPreliminaryResults = appController.ExamResultController.HasPreliminaryResults(student);
                return !hasPreliminaryResults;
            }
            return false;
        }
    }
}
