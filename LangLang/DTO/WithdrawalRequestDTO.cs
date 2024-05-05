using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using System;

namespace LangLang.DTO
{
    public class WithdrawalRequestDTO
    {   
        public int Id { get; set; }
        public int EnrollmentRequestId;
        public Status Status;
        public DateTime RequestSentAt;
        public DateTime RequestReceivedAt;
        public Student Student { get; set; }
        public string Reason { get; set; }

        public WithdrawalRequestDTO() {}

        public WithdrawalRequestDTO(WithdrawalRequest request, AppController appController)
        {
            Id = request.Id;
            EnrollmentRequestId = request.EnrollmentRequestId;
            Reason = request.Reason;
            Status = request.Status;
            RequestSentAt = request.RequestSentAt;
            RequestReceivedAt = request.RequestReceivedAt;
            Student = appController.StudentController.Get(appController.EnrollmentRequestController.GetById(EnrollmentRequestId).StudentId);
        }

        public WithdrawalRequest ToWithdrawalRequest()
        {
            return new WithdrawalRequest(Id, EnrollmentRequestId, Reason, Status, RequestSentAt, RequestReceivedAt);
        }

    }
}
