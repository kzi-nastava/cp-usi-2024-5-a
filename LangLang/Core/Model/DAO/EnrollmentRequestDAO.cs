using LangLang.Core.Observer;
using LangLang.Core.Repository;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Model.DAO
{
    public class EnrollmentRequestDAO : Subject
    {
        private readonly Dictionary<int, EnrollmentRequest> _enrollmentRequests;
        private readonly Repository<EnrollmentRequest> _repository;

        public EnrollmentRequestDAO()
        {
            _repository = new Repository<EnrollmentRequest>("enrollmentRequests.csv");
            _enrollmentRequests = _repository.Load();
        }
        private int GenerateId()
        {
            if (_enrollmentRequests.Count == 0) return 0;
            return _enrollmentRequests.Count + 1;
        }

        public EnrollmentRequest? GetEnrollmentRequestById(int id)
        {
            return _enrollmentRequests[id];
        }

        public Dictionary<int, EnrollmentRequest> GetAllEnrollmentRequests()
        {
            return _enrollmentRequests;
        }

        public EnrollmentRequest Add(EnrollmentRequest enrollmentRequest)
        {
            enrollmentRequest.Id = GenerateId();
            _enrollmentRequests.Add(enrollmentRequest.Id, enrollmentRequest);
            _repository.Save(_enrollmentRequests);
            NotifyObservers();
            return enrollmentRequest;
        }

        public EnrollmentRequest? Update(EnrollmentRequest enrollmentRequest)
        {
            EnrollmentRequest oldRequest = GetEnrollmentRequestById(enrollmentRequest.Id);
            if (oldRequest == null) return null;

            oldRequest.Status = enrollmentRequest.Status;

            _repository.Save(_enrollmentRequests);
            NotifyObservers();
            return oldRequest;
        }

        public EnrollmentRequest? Remove(int id)
        {
            EnrollmentRequest enrollmentRequest = GetEnrollmentRequestById(id);
            if (enrollmentRequest == null) return null; 

            _enrollmentRequests.Remove(enrollmentRequest.Id);
            _repository.Save(_enrollmentRequests);
            NotifyObservers();
            return enrollmentRequest;
        }

        public List<EnrollmentRequest> GetStudentRequests(int studentId)
        {
            Dictionary<int, EnrollmentRequest> studentRequests = new();
            foreach (EnrollmentRequest enrollmentRequest in GetAllEnrollmentRequests().Values)
            {
                if (enrollmentRequest.StudentId == studentId) studentRequests.Add(enrollmentRequest.Id, enrollmentRequest);

            }
            return studentRequests.Values.ToList();
        }
    }
}
