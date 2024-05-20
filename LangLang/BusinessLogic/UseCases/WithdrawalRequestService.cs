
using LangLang.Composition;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.BusinessLogic.UseCases
{
    public class WithdrawalRequestService
    {
        private IWithdrawalRequestRepository _withdrawalRequests;
        public WithdrawalRequestService() {
            _withdrawalRequests = Injector.CreateInstance<IWithdrawalRequestRepository>();
        }

        public WithdrawalRequest Get(int id)
        {
            return _withdrawalRequests.Get(id);
        }

        public List<WithdrawalRequest> GetAll()
        {
            return _withdrawalRequests.GetAll();
        }


        public void Add(WithdrawalRequest request)
        {
            _withdrawalRequests.Add(request);
        }

        public void Delete(int id)
        {
            _withdrawalRequests.Delete(id);
        }

        public void Update(WithdrawalRequest request)
        {
            _withdrawalRequests.Update(request);
        }

        public void UpdateStatus(int requestId, Status newStatus)
        {
            var request = Get(requestId);
            request.UpdateStatus(newStatus);
            _withdrawalRequests.Update(request);
        }

        public List<WithdrawalRequest> GetByStudent(Student student)
        {
            return _withdrawalRequests.GetByStudent(student);
        }

        public List<WithdrawalRequest> GetByCourse(Course course)
        {
            return _withdrawalRequests.GetByCourse(course);
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

        public void Save()
        {
            _withdrawalRequests.Save();
        }

        public Dictionary<int, WithdrawalRequest> Load()
        {
            return _withdrawalRequests.Load();
        }
    }
}
