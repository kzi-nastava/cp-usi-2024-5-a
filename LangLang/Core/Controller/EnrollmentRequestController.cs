using LangLang.Core.Model.DAO;
using System.Collections.Generic;
using LangLang.Core.Model;
using LangLang.Core.Observer;

namespace LangLang.Core.Controller
{
    public class EnrollmentRequestController
    {
        private readonly EnrollmentRequestDAO _enrollmentRequests;

        public EnrollmentRequestController()
        {
            _enrollmentRequests = new EnrollmentRequestDAO();
        }

        public List<EnrollmentRequest> GetAll()
        {
            return _enrollmentRequests.GetAllEnrollmentRequests();
        }

        public EnrollmentRequest GetById(int id)
        { 
            return _enrollmentRequests.GetById(id);
        }

        public void Add(EnrollmentRequest enrollmentRequest)
        {
            _enrollmentRequests.Add(enrollmentRequest);
        }

        public void Delete(int id)
        {
            _enrollmentRequests.Remove(id);
        }

        public void Update(EnrollmentRequest enrollmentRequest)
        {
            _enrollmentRequests.Update(enrollmentRequest);
        }

        public void Subscribe(IObserver observer)
        {
            _enrollmentRequests.Subscribe(observer);
        }

        public List<EnrollmentRequest> GetStudentRequests(int studentId)
        {
            return _enrollmentRequests.GetStudentRequests(studentId);
        }

        public bool CancelRequest(EnrollmentRequest enrollmentRequest, CourseController courseController)
        {
            Course course = courseController.GetById(enrollmentRequest.CourseId);
            return _enrollmentRequests.CancelRequest(enrollmentRequest, course);
        }

        // this method is invoked when the tutor approves the request for the student
        public void PauseRequests(int studentId, EnrollmentRequest enrollmentRequest)
        {
            _enrollmentRequests.PauseRequests(studentId, enrollmentRequest.Id);
        }

        // this method is invoked when the student complete/withdrawal_from course
        public void ResumePausedRequests(int studentId)
        {
            _enrollmentRequests.ResumePausedRequests(studentId);
        }

        public bool CanRequestWithdrawal(int id)
        {
            return _enrollmentRequests.CanRequestWithdrawal(id);
        }

        public EnrollmentRequest? GetActiveCourseRequest(int studentId, CourseController courseController, WithdrawalRequestController wrController)
        {
            return _enrollmentRequests.GetActiveCourseRequest(studentId, courseController, wrController);
        }
        
    }
}