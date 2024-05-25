using LangLang.Core;
using LangLang.Core.Model.Enums;
using System;
namespace LangLang.Domain.Models
{
    public class ExamApplication
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamSlotId { get; set; }
        public DateTime SentAt { get; set; }

        public ExamApplication() { }

        public ExamApplication(int id, int studentId, int examSlotId, DateTime sentAt)
        {
            Id = id;
            StudentId = studentId;
            ExamSlotId = examSlotId;
            SentAt = sentAt;
        }
       
    }
}
