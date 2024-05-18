using LangLang.Aplication.UseCases;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using LangLang.Domain.Models;
using System;

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
        public DateTime LastModifiedAt { get; set; }
        public bool IsCanceled {  get; set; }
        public Course Course {  get; set; }

        public EnrollmentRequest ToEnrollmentRequest()
        {
            return new EnrollmentRequest(Id, StudentId, CourseId, Status, RequestSentAt, LastModifiedAt, IsCanceled);
        }

        public EnrollmentRequestDTO(EnrollmentRequest enrollmentRequest, AppController appController)
        {
            Id = enrollmentRequest.Id;
            CourseId = enrollmentRequest.CourseId;
            StudentId = enrollmentRequest.StudentId;
            var studentService = new StudentService();
            Student student = studentService.Get(StudentId);

            Status = enrollmentRequest.Status;
            RequestSentAt = enrollmentRequest.RequestSentAt;
            LastModifiedAt = enrollmentRequest.LastModifiedAt;
            IsCanceled = enrollmentRequest.IsCanceled;
            Course = appController.CourseController.Get(CourseId);

            StudentName = student.Profile.Name;
            StudentLastName = student.Profile.LastName;
            Email = student.Profile.Email;
            PhoneNumber = student.Profile.PhoneNumber;
            BirthDate = student.Profile.BirthDate.ToString("MM/dd/yyyy");
        }

    }
}
