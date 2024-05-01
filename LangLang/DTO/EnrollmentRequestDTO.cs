using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using System;
using System.ComponentModel;

namespace LangLang.DTO
{
    public class EnrollmentRequestDTO : INotifyPropertyChanged
    {
        public EnrollmentRequestDTO() { }

        public int Id { get; set; }

        public int CourseId { get; set; }
        public int StudentId {  get; set; }
        private Status status;
        private DateTime requestSentAt;
        private DateTime lastModifiedTimestamp;
        private bool isCanceled;

        public Status Status
        {
            get { return status; }
            set { status = value; }
        }

        public DateTime RequestSentAt
        {
            get { return requestSentAt; }
            set { requestSentAt = value; }
        }

        public DateTime LastModifiedTimestamp
        {
            get { return lastModifiedTimestamp; }
            set { lastModifiedTimestamp = value; }
        }
        public bool IsCanceled
        {
            get { return isCanceled; }
            set { isCanceled = value; }
        }
        
        public EnrollmentRequest ToEnrollmentRequest()
        {
            return new EnrollmentRequest(Id, StudentId, CourseId, status, requestSentAt, lastModifiedTimestamp, isCanceled);
        }

        public EnrollmentRequestDTO(EnrollmentRequest enrollmentRequest, AppController appController)
        {
            Id = enrollmentRequest.Id;
            CourseId = enrollmentRequest.CourseId;
            StudentId = enrollmentRequest.StudentId;
            status = enrollmentRequest.Status;
            requestSentAt = enrollmentRequest.RequestSentAt;
            lastModifiedTimestamp = enrollmentRequest.LastModifiedTimestamp;
            isCanceled = enrollmentRequest.IsCanceled;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
