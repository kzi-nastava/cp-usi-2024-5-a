using LangLang.Aplication.UseCases;
using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Model.Enums;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
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
            return _withdrawalRequests.GetAll();
        }

        public WithdrawalRequest Get(int id)
        {
            return _withdrawalRequests.Get(id);
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

        public List<WithdrawalRequest> GetRequests(Student student)
        {
            var enrollmentReqService = new EnrollmentRequestService(); 
            var allEnrollmentRequests = enrollmentReqService.GetAll();
            return _withdrawalRequests.GetRequests(student, allEnrollmentRequests);
        }

        public List<WithdrawalRequest> GetRequests(Course course)
        {
            var enrollmentReqService = new EnrollmentRequestService();
            var allEnrollmentRequests = enrollmentReqService.GetAll();
            return _withdrawalRequests.GetRequests(course, allEnrollmentRequests);
        }

        public bool AlreadyExists(int enrollmentRequestId)
        {
            return _withdrawalRequests.AlreadyExists(enrollmentRequestId);
        }

        public bool HasAcceptedWithdrawal(int enrollmentRequestId)
        {
            return _withdrawalRequests.HasAcceptedWithdrawal(enrollmentRequestId);
        }

        public void UpdateStatus(int id, Status status)
        {
            _withdrawalRequests.UpdateStatus(id, status);
        }
    }
}
