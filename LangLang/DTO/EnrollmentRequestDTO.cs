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

        private Course course;
        private Student student;
        private Status status;
        private DateTime requestSentAt;
        private DateTime lastModifiedTimestamp;
        private bool isCanceled;

        public EnrollmentRequest ToEnrollmentRequest()
        {
            return new EnrollmentRequest(Id, student.Id, course.Id, status, requestSentAt, lastModifiedTimestamp, isCanceled);
        }

        public EnrollmentRequestDTO(EnrollmentRequest enrollmentRequest, AppController appController)
        {
            Id = enrollmentRequest.Id;
            course = appController.CourseController.GetById(enrollmentRequest.CourseId);
            student = appController.StudentController.GetById(enrollmentRequest.StudentId);
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
