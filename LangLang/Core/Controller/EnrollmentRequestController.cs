using LangLang.Core.Model.DAO;
using System.Collections.Generic;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using LangLang.Domain.Models;

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
            return _enrollmentRequests.GetAll();
        }

        public EnrollmentRequest Get(int id)
        {
            return _enrollmentRequests.Get(id);
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

        public List<EnrollmentRequest> GetRequests(Student student)
        {
            return _enrollmentRequests.GetRequests(student);
        }

        public List<EnrollmentRequest> GetRequests(Course course)
        {
            return _enrollmentRequests.GetRequests(course);
        }

        public bool CancelRequest(EnrollmentRequest enrollmentRequest, CourseController courseController)
        {
            Course course = courseController.Get(enrollmentRequest.CourseId);
            return _enrollmentRequests.CancelRequest(enrollmentRequest.Id, course);
        }

        // this method is invoked when the tutor approves the request for the student
        public void PauseRequests(Student student, EnrollmentRequest acceptedRequest)
        {
            _enrollmentRequests.PauseRequests(student, acceptedRequest.Id);
        }

        // this method is invoked when the student complete/withdrawal_from course
        public void ResumePausedRequests(Student student)
        {
            _enrollmentRequests.ResumePausedRequests(student);
        }

        public bool CanRequestWithdrawal(int id)
        {
            return _enrollmentRequests.CanRequestWithdrawal(id);
        }

        public EnrollmentRequest? GetActiveCourseRequest(Student student, AppController appController)
        {
            return _enrollmentRequests.GetActiveCourseRequest(student, appController);
        }

        public bool AlreadyExists(Student student, Course course)
        {
            return _enrollmentRequests.AlreadyExists(student, course);
        }

    }
}