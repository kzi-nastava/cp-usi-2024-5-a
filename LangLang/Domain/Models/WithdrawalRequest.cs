using System;
using LangLang.Domain.Enums;


namespace LangLang.Domain.Models
{
    // this class represents student's request to withdraw from course
    public class WithdrawalRequest 
    {
        public int Id { get; set; }
        public int EnrollmentRequestId { get; set; }
        public string Reason { get; set; }
        public Status Status { get; private set; }
        public DateTime RequestSentAt { get; set; }
        public DateTime RequestReceivedAt { get; private set; }

        public WithdrawalRequest() { }
        public WithdrawalRequest(int id, int enrollmentRequestId, string reason, Status status, DateTime requestSentAt, DateTime requestReceivedAt)
        {
            Id = id;
            EnrollmentRequestId = enrollmentRequestId;
            Reason = reason;
            Status = status;
            RequestSentAt = requestSentAt;
            RequestReceivedAt = requestReceivedAt;
        }

        public void UpdateStatus(Status status)
        {
            Status = status;
            RequestReceivedAt = DateTime.Now;
        }
    }
}
