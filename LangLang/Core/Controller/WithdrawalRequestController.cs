
using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Observer;
using System.Collections.Generic;

namespace LangLang.Core.Controller
{
    public class WithdrawalRequestController
    {
        private readonly WithdrawalRequestDAO _withdrawalRequests;

        public WithdrawalRequestController()
        {
            _withdrawalRequests = new();
        }

        public List<WithdrawalRequest> GetAll()
        {
            return _withdrawalRequests.GetAllWithdrawalRequests();
        }

        public WithdrawalRequest GetById(int id)
        {
            return _withdrawalRequests.GetById(id);
        }

        public void Add(WithdrawalRequest withdrawalRequest)
        {
            _withdrawalRequests.Add(withdrawalRequest);
        }

        public void Delete(int id)
        {
            _withdrawalRequests.Remove(id);
        }

        public void Subscribe(IObserver observer)
        {
            _withdrawalRequests.Subscribe(observer);
        }

        public List<WithdrawalRequest> GetStudentRequests(int studentId, EnrollmentRequestController erController)
        {
            List<EnrollmentRequest> allEnrollmentRequests = erController.GetAll();
            return _withdrawalRequests.GetStudentRequests(studentId, allEnrollmentRequests);
        }
    }
}
