using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using System;
using System.ComponentModel;

namespace LangLang.DTO
{
    public class ExamAppRequestDTO
    {
        public ExamAppRequestDTO() { }

        public int Id { get; set; }

        public int ExamSlotId { get; set; }
        public int StudentId { get; set; }
        public DateTime SentAt { get; set; }
        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Language { get; set; }
        public LanguageLevel Level { get; set; }
        public DateTime ExamDateTime { get; set; }


        public ExamAppRequest ToExamAppRequest()
        {
            return new ExamAppRequest(Id, StudentId, ExamSlotId, SentAt);
        }

        public ExamAppRequestDTO(ExamAppRequest appRequest, AppController appController)
        {
            ExamSlot exam = appController.ExamSlotController.GetById(appRequest.ExamSlotId);
            Student student = appController.StudentController.Get(appRequest.StudentId);
            Id = appRequest.Id;
            ExamSlotId = appRequest.ExamSlotId;
            StudentId = appRequest.StudentId;
            SentAt = appRequest.SentAt;

            StudentName = student.Profile.Name;
            StudentLastName = student.Profile.LastName;
            Email = student.Profile.Email;
            PhoneNumber = student.Profile.PhoneNumber;

            Language = exam.Language;
            Level = exam.Level;
            ExamDateTime = exam.TimeSlot.Time;
        }

    }
}
