using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using System;
using System.ComponentModel;

namespace LangLang.DTO
{
    public class EnrollmentRequestDTO
    {
        public EnrollmentRequestDTO() { }

        public int Id { get; set; }

        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public Status Status { get; set;}
        public DateTime RequestSentAt {  get; set; }
        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }
        public bool IsCanceled {  get; set; }
        public Course Course {  get; set; }

        public EnrollmentRequest ToEnrollmentRequest()
        {
            return new EnrollmentRequest(Id, StudentId, CourseId, Status, RequestSentAt, LastModifiedTimestamp, IsCanceled);
        }

        public EnrollmentRequestDTO(EnrollmentRequest enrollmentRequest, AppController appController)
        {
            Id = enrollmentRequest.Id;
            CourseId = enrollmentRequest.CourseId;
            StudentId = enrollmentRequest.StudentId;
            Student student = appController.StudentController.GetById(StudentId);

            Status = enrollmentRequest.Status;
            RequestSentAt = enrollmentRequest.RequestSentAt;
            LastModifiedTimestamp = enrollmentRequest.LastModifiedTimestamp;
            IsCanceled = enrollmentRequest.IsCanceled;
            Course = appController.CourseController.GetById(CourseId);

            StudentName = student.Profile.Name;
            StudentLastName = student.Profile.LastName;
            Email = student.Profile.Email;
            PhoneNumber = student.Profile.PhoneNumber;
            BirthDate = student.Profile.BirthDate.Date.ToString();
        }

    }
}
