
using LangLang.Core.Model.Enums;
using LangLang.Core.Repository.Serialization;
using System;

namespace LangLang.Core.Model
{
    // this class represents student's request to withdraw from course
    public class WithdrawalRequest : ISerializable
    {
        public int Id { get; set; }
        public int EnrollmentRequestId { get; set; }
        public string Reason { get; set; }
        public Status Status { get; private set; }
        public DateTime RequestSentAt { get; set; }
        public DateTime RequestReceivedAt { get; private set;}

        public WithdrawalRequest() {}
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

        public void FromCSV(string[] values)
        {
            try
            {
                RequestSentAt = DateTime.ParseExact(values[4], "yyyy-MM-dd", null);
                RequestReceivedAt = DateTime.ParseExact(values[5], "yyyy-MM-dd", null);
            }
            catch
            {
                throw new FormatException("Date is not in the correct format.");
            }

            Id = int.Parse(values[0]);
            EnrollmentRequestId = int.Parse(values[1]);
            Reason = values[2];
            Status = (Status)Enum.Parse(typeof(Status), values[3]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                EnrollmentRequestId.ToString(),
                Reason,
                Status.ToString(),
                RequestSentAt.ToString("yyyy-MM-dd"),
                RequestReceivedAt.ToString("yyyy-MM-dd"),
            };
        }
    }
}
