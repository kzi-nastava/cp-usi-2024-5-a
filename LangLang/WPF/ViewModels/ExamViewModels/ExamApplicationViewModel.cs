using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System;

namespace LangLang.WPF.ViewModels.ExamViewModel
{
    public class ExamApplicationViewModel
    {
        public ExamApplicationViewModel() { }

        public int Id { get; set; }

        public int ExamSlotId { get; set; }
        public int StudentId { get; set; }
        public DateTime SentAt { get; set; }
        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Language { get; set; }
        public Level Level { get; set; }
        public DateTime ExamDateTime { get; set; }


        public ExamApplication ToExamApplication()
        {
            return new ExamApplication(Id, StudentId, ExamSlotId, SentAt);
        }

        public ExamApplicationViewModel(ExamApplication application)
        {
            var examService = new ExamSlotService();
            var timeService = new TimeSlotService();
            var studentService = new StudentService();
            var languageService = new LanguageLevelService();

            ExamSlot exam = examService.Get(application.ExamSlotId);
            Student student = studentService.Get(application.StudentId);
            LanguageLevel language = languageService.Get(exam.LanguageId);

            Id = application.Id;
            ExamSlotId = application.ExamSlotId;
            StudentId = application.StudentId;
            SentAt = application.SentAt;

            StudentName = student.Profile.Name;
            StudentLastName = student.Profile.LastName;
            Email = student.Profile.Email;
            PhoneNumber = student.Profile.PhoneNumber;

            Language = language.Language;
            Level = language.Level;
            ExamDateTime = timeService.Get(exam.TimeSlotId).Time;
        }

    }
}
