using LangLang.Core.Model.Enums;
using LangLang.Core.Repository.Serialization;
using System;

namespace LangLang.Core.Model
{
    public class EnrollmentRequest : ISerializable
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public Status Status { get; set; }
        public DateTime RequestSentAt { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }

        public EnrollmentRequest() { }

        public EnrollmentRequest(int id, int studentId, int courseId, Status erStatus, DateTime requestSentAt)
        {
            Id = id;
            StudentId = studentId;
            CourseId = courseId;
            Status = erStatus;
            RequestSentAt = requestSentAt;
        }

        public void FromCSV(string[] values)
        {
            try {
                RequestSentAt = DateTime.ParseExact(values[4], "yyyy-MM-dd", null);
                LastModifiedTimestamp = DateTime.ParseExact(values[5], "yyyy-MM-dd", null);
            }
            catch {
                throw new FormatException("Date is not in the correct format.");
            }

            Id = int.Parse(values[0]);
            StudentId = int.Parse(values[1]);
            CourseId = int.Parse(values[2]);
            Status = (Status)Enum.Parse(typeof(Status), values[3]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                StudentId.ToString(),
                CourseId.ToString(),
                Status.ToString(),
                RequestSentAt.ToString("yyyy-MM-dd"),
                LastModifiedTimestamp.ToString("yyyy-MM-dd")
            };
        }
    }
}
