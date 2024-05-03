using LangLang.Core.Model.Enums;
using LangLang.Core.Repository.Serialization;
using System;
namespace LangLang.Core.Model
{
    public class ExamAppRequest : ISerializable
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamSlotId { get; set; }
        public DateTime RequestSentAt { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }
        public bool IsCanceled { get; set; }

        public ExamAppRequest() { }

        public ExamAppRequest(int id, int studentId, int examSlotId, DateTime requestSentAt)
        {
            Id = id;
            StudentId = studentId;
            ExamSlotId = examSlotId;
            RequestSentAt = requestSentAt;
            LastModifiedTimestamp = requestSentAt; //Later this will refer to the date of cancellation if student cancelled
            IsCanceled = false;
        }

        public void CancelExamAppRequest()
        {
            IsCanceled = true;
            LastModifiedTimestamp = DateTime.Now;
        }

        public void FromCSV(string[] values)
        {
            try
            {
                RequestSentAt = DateTime.ParseExact(values[4], "yyyy-MM-dd", null);
                LastModifiedTimestamp = DateTime.ParseExact(values[5], "yyyy-MM-dd", null);
            }
            catch
            {
                throw new FormatException("Date is not in the correct format.");
            }

            Id = int.Parse(values[0]);
            StudentId = int.Parse(values[1]);
            ExamSlotId = int.Parse(values[2]);
            IsCanceled = bool.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                StudentId.ToString(),
                ExamSlotId.ToString(),
                IsCanceled.ToString(),
                RequestSentAt.ToString("yyyy-MM-dd"),
                LastModifiedTimestamp.ToString("yyyy-MM-dd"),
            };
        }
    }
}
