
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

        public Dictionary<int, EnrollmentRequest> GetAll()
        {
            return _enrollmentRequests.GetAllEnrollmentRequests();
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

        public Dictionary<int, EnrollmentRequest> GetStudentRequests(int studentId)
        {
            Dictionary<int, EnrollmentRequest> studentRequests = new();
            foreach (EnrollmentRequest enrollmentRequest in  _enrollmentRequests.GetAllEnrollmentRequests().Values)
            {
                if (enrollmentRequest.StudentId == studentId) studentRequests.Add(enrollmentRequest.Id, enrollmentRequest);

            }
            return studentRequests;
        }
    }
}