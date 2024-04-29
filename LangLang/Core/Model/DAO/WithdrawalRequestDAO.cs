
using LangLang.Core.Observer;
using LangLang.Core.Repository;
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

        public WithdrawalRequest GetById(int id)
        {
            return _withdrawalRequests[id];
        }

        public List<WithdrawalRequest> GetAllWithdrawalRequests() 
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
            WithdrawalRequest? request = GetById(id);
            if (request == null) return null;

            _withdrawalRequests.Remove(request.Id);
            _repository.Save(_withdrawalRequests);
            NotifyObservers();
            return request;
        }

        public List<WithdrawalRequest> GetStudentRequests(int studentId, List<EnrollmentRequest> allEnrollmentRequests)
        {
            List<WithdrawalRequest> studentRequests = new();
            foreach (WithdrawalRequest request in GetAllWithdrawalRequests())
            {
                EnrollmentRequest enrollmentRequest = allEnrollmentRequests[request.EnrollmentRequestId];
                if (enrollmentRequest.StudentId == studentId)
                {
                    studentRequests.Add(request);
                }
            }
            return studentRequests;
        }

    }
}
