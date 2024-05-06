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
        public Status Status { get; private set; }
        public DateTime RequestSentAt { get; set; }
        public DateTime LastModifiedAt { get; private set; }
        public bool IsCanceled { get; private set; }

        public EnrollmentRequest() { }

        public EnrollmentRequest(int id, int studentId, int courseId, Status status, DateTime requestSentAt)
        {
            Id = id;
            StudentId = studentId;
            CourseId = courseId;
            Status = status;
            RequestSentAt = requestSentAt;
            LastModifiedAt = requestSentAt; //Later this will refer to the date of acceptance/rejection/cancellation
            IsCanceled = false; // this refers whether the student has canceled request before the course has started  
        }

        public EnrollmentRequest(int id, int studentId, int courseId, Status status, DateTime requestSentAt, DateTime lastModifiedAt, bool isCanceled) : this(id, studentId, courseId, status, requestSentAt)
        {
            LastModifiedAt = lastModifiedAt;
            IsCanceled = isCanceled;
        }   

        public void UpdateStatus(Status status)
        {
            Status = status;
            LastModifiedAt = DateTime.Now;
        }

        public void CancelRequest()
        {
            IsCanceled = true;
            LastModifiedAt = DateTime.Now;
        }

        public bool CanWithdraw()
        {
            return (DateTime.Now - LastModifiedAt).Days > Constants.COURSE_CANCELLATION_PERIOD;
        }

        public void FromCSV(string[] values)
        {
            try {
                RequestSentAt = DateTime.ParseExact(values[4], Constants.DATE_FORMAT, null);
                LastModifiedAt = DateTime.ParseExact(values[5], Constants.DATE_FORMAT, null);
            }
            catch {
                throw new FormatException("Date is not in the correct format.");
            }

            Id = int.Parse(values[0]);
            StudentId = int.Parse(values[1]);
            CourseId = int.Parse(values[2]);
            Status = (Status)Enum.Parse(typeof(Status), values[3]);
            IsCanceled = bool.Parse(values[6]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                StudentId.ToString(),
                CourseId.ToString(),
                Status.ToString(),
                RequestSentAt.ToString(Constants.DATE_FORMAT),
                LastModifiedAt.ToString(Constants.DATE_FORMAT),
                IsCanceled.ToString()
            };
        }
    }
}
