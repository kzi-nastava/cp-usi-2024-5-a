
using LangLang.Core.Model.DAO;
using System.Collections.Generic;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using System.Linq;

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

        public List<EnrollmentRequest> GetStudentRequests(int studentId)
        {
            return _enrollmentRequests.GetStudentRequests(studentId);
        }
    }
}