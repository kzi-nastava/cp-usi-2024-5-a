using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System.Collections.Generic;
using LangLang.Domain.RepositoryInterfaces;
using System.Linq;
using LangLang.Composition;

namespace LangLang.BusinessLogic.UseCases
{
    public class StudentService
    {
        private IStudentRepository _students;

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
            var student = Get(id);
            var enrollmentService = new EnrollmentRequestService();
            var examAppService = new ExamApplicationService();

            foreach (EnrollmentRequest er in enrollmentService.GetByStudent(student)) // delete all course enrollment requests
                enrollmentService.Delete(er.Id);

            foreach (ExamApplication ar in examAppService.GetApplications(student)) // delete all exam application requests
                examAppService.Delete(ar.Id);
            _students.Deactivate(id);
        }

        public bool CanModifyData(Student student)
        {
            // can modify - student is not currently enrolled in any course and has not applied for any exams
            return (CanRequestEnrollment(student) && !HasAppliedForExam(student.Id));
        }

        public bool CanRequestEnrollment(Student student)
        {
            var enrollmentService = new EnrollmentRequestService();
            var courseService = new CourseService();
            var withdrawalService = new WithdrawalRequestService();

            foreach (EnrollmentRequest er in enrollmentService.GetByStudent(student))
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
            var examAppService = new ExamApplicationService();
            List<ExamApplication> requests = examAppService.GetActiveStudentApplications(studentId);

            return requests.Count != 0;
        }

        public bool CanApplyForCourses(Student student)
        {
            var examAppService = new ExamApplicationService();
            var examResultService = new ExamResultService();

            bool hasNoResults = examAppService.HasNoGeneratedResults(student);
            bool hasNotGradedExams = examResultService.HasNotGradedResults(student);
            return !hasNoResults && !hasNotGradedExams;
        }

        public bool CanApplyForExams(Student student)
        {
            var examResultService = new ExamResultService();
            if (CanApplyForCourses(student))
            {
                bool hasPreliminaryResults = examResultService.HasPreliminaryResults(student);
                return !hasPreliminaryResults;
            }
            return false;
        }
    }
}
