﻿using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using System;
using System.ComponentModel;

namespace LangLang.DTO
{
    public class ExamApplicationDTO
    {
        public ExamApplicationDTO() { }

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


        public ExamApplication ToExamApplication()
        {
            return new ExamApplication(Id, StudentId, ExamSlotId, SentAt);
        }

        public ExamApplicationDTO(ExamApplication application, AppController appController)
        {
            ExamSlot exam = appController.ExamSlotController.Get(application.ExamSlotId);
            Student student = appController.StudentController.Get(application.StudentId);
            Id = application.Id;
            ExamSlotId = application.ExamSlotId;
            StudentId = application.StudentId;
            SentAt = application.SentAt;

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