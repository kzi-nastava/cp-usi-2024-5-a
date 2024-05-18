using LangLang.Core.Model.Enums;
using LangLang.Core.Observer;
using LangLang.Core.Repository;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Linq;


namespace LangLang.Core.Model.DAO
{
    public class WithdrawalRequestDAO : Subject
    {
        private readonly Dictionary<int, WithdrawalRequest> _withdrawalRequests;
        private readonly Repository<WithdrawalRequest> _repository;
    
        public WithdrawalRequestDAO()
        {
            _repository = new Repository<WithdrawalRequest>("withdrawalRequests.csv");
            _withdrawalRequests = _repository.Load();
        }

        private int GenerateId()
        {
            if (_withdrawalRequests.Count == 0) return 0;
            return _withdrawalRequests.Keys.Max() + 1;
        }

        public WithdrawalRequest Get(int id)
        {
            return _withdrawalRequests[id];
        }

        public List<WithdrawalRequest> GetAll() 
        {
            return _withdrawalRequests.Values.ToList();
        }

        public WithdrawalRequest Add(WithdrawalRequest request)
        {
            request.Id= GenerateId();
            _withdrawalRequests.Add(request.Id, request);
            _repository.Save(_withdrawalRequests);
            NotifyObservers();
            return request;
        }

        public WithdrawalRequest? Remove(int id)
        {
            WithdrawalRequest? request = Get(id);
            if (request == null) return null;

            _withdrawalRequests.Remove(request.Id);
            _repository.Save(_withdrawalRequests);
            NotifyObservers();
            return request;
        }

        public List<WithdrawalRequest> GetRequests(Student student, List<EnrollmentRequest> allEnrollmentRequests)
        {
            List<WithdrawalRequest> studentRequests = new();
            foreach (WithdrawalRequest request in GetAll())
            {
                EnrollmentRequest enrollmentRequest = allEnrollmentRequests[request.EnrollmentRequestId];
                if (enrollmentRequest.StudentId == student.Id)
                {
                    studentRequests.Add(request);
                }
            }
            return studentRequests;
        }

        public List<WithdrawalRequest> GetRequests(Course course, List<EnrollmentRequest> allEnrollmentRequests)
        {
            List<WithdrawalRequest> courseRequests = new();
            foreach (WithdrawalRequest request in GetAll())
            {
                EnrollmentRequest enrollmentRequest = allEnrollmentRequests[request.EnrollmentRequestId];
                if (enrollmentRequest.CourseId == course.Id)
                {
                    courseRequests.Add(request);
                }
            }
            return courseRequests;
        }

        public bool AlreadyExists(int enrollmentRequestId)
        {
            return GetAll().Any(wr => wr.EnrollmentRequestId == enrollmentRequestId);
        }

        public bool HasAcceptedWithdrawal(int enrollmentRequestId)
        {
            return GetAll().Any(wr => wr.EnrollmentRequestId == enrollmentRequestId 
                                && wr.Status == Status.Accepted);
        }

        public void UpdateStatus(int id, Status status)
        {
            WithdrawalRequest request = _withdrawalRequests[id];
            request.UpdateStatus(status);
            _repository.Save(_withdrawalRequests);
        }

    }
}
