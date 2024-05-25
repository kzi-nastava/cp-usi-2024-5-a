using LangLang.Configuration;
using System;
using LangLang.Domain.Enums;

namespace LangLang.Domain.Models
{
    public class EnrollmentRequest 
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
    }
}
