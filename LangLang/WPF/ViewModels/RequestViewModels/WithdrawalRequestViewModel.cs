using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.Domain.Enums;
using System;

namespace LangLang.WPF.ViewModels.RequestViewModels
{
    public class WithdrawalRequestViewModel
    {
        public int Id { get; set; }
        public int EnrollmentRequestId;
        public Status Status;
        public DateTime RequestSentAt;
        public DateTime RequestReceivedAt;
        public Student Student { get; set; }
        public string Reason { get; set; }

        public WithdrawalRequestViewModel() { }

        public WithdrawalRequestViewModel(WithdrawalRequest request)
        {
            Id = request.Id;
            EnrollmentRequestId = request.EnrollmentRequestId;
            Reason = request.Reason;
            Status = request.Status;
            RequestSentAt = request.RequestSentAt;
            RequestReceivedAt = request.RequestReceivedAt;
            SetStudent();
        }

        private void SetStudent()
        {
            var studentService = new StudentService();
            var enrollmentReqService = new EnrollmentRequestService();
            var enrollmentRequest = enrollmentReqService.Get(EnrollmentRequestId);

            Student = studentService.Get(enrollmentRequest.StudentId);
        }

        public WithdrawalRequest ToWithdrawalRequest()
        {
            return new WithdrawalRequest(Id, EnrollmentRequestId, Reason, Status, RequestSentAt, RequestReceivedAt);
        }

    }
}
