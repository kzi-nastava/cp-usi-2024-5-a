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

        public void Deactivate(int id) 
        {
            _students.Deactivate(id);
        }

        public void Subscribe(IObserver observer)
        {
            _students.Subscribe(observer);
        }

        public bool CanModifyData(Student student)
        {
            // can modify - student is not currently enrolled in any course and has not applied for any exams
            return (CanRequestEnrollment(student) && !HasAppliedForExam(student.Id));
        }

        public bool CanRequestEnrollment(Student student)
        {
            var enrollmentService = new EnrollmentRequestController();
            var courseService = new CourseController();
            var withdrawalService = new WithdrawalRequestController();

            foreach (EnrollmentRequest er in enrollmentService.GetRequests(student))
            {
                if (er.Status == Status.Accepted && !er.IsCanceled)
                {
                    if (!courseService.IsCompleted(er.CourseId) && !withdrawalService.HasAcceptedWithdrawal(er.Id))
                        return false;
                }
            }
            return true;
        }

        public bool HasAppliedForExam(int studentId)
        {
            var examAppService = new ExamApplicationController();
            var examService = new ExamSlotController();
            List<ExamApplication> requests = examAppService.GetActiveStudentApplications(studentId);

            return requests.Count != 0;
        }

        public bool CanApplyForCourses(Student student)
        {
            var examAppService = new ExamApplicationController();
            var examResultService = new ExamResultController();
            bool hasNoResults = examAppService.HasNoGeneratedResults(student);
            bool hasNotGradedExams = examResultService.HasNotGradedResults(student);
            return !hasNoResults && !hasNotGradedExams;
        }

        public bool CanApplyForExams(Student student)
        {
            var examResultService = new ExamResultController();
            if (CanApplyForCourses(student))
            {
                bool hasPreliminaryResults = examResultService.HasPreliminaryResults(student);
                return !hasPreliminaryResults;
            }
            return false;
        }
    }
}
