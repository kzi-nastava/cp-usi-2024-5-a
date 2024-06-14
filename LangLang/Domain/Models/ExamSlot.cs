using LangLang.Configuration;
using LangLang.ConsoleApp.Attributes;
using LangLang.Domain.Enums;
using System;
using System.ComponentModel;

namespace LangLang.Domain.Models
{
    public class ExamSlot
    {
        public int Id { get; set; }
        [Show]
        public string Language { get; set; }
        [Show]
        [DisplayName("Language level")]
        public LanguageLevel Level { get; set; }
        [Show]
        [DisplayName("Tutor")]
        public int TutorId { get; set; }
        [Show]
        [AllowUpdate]
        [DisplayName("Exam time")]
        public TimeSlot TimeSlot { get; set; }
        [Show]
        [AllowUpdate]
        [DisplayName("Max Students")]
        public int MaxStudents { get; set; }
        [Show]
        public int Applicants { get; set; }
        public bool Modifiable { get; set; }
        public bool ResultsGenerated { get; set; }
        public bool ExamineesNotified { get; set; }
        public DateTime CreatedAt { get; set; }

        public ExamSlot(int id, string language, LanguageLevel level, TimeSlot timeSlot, int maxStudents, int tutorId, int applicants, bool modifiable, bool generatedResults, bool examineesNotified, DateTime createdAt)
        {
            Id = id;
            Language = language;
            Level = level;
            TutorId = tutorId;
            TimeSlot = timeSlot;
            MaxStudents = maxStudents;
            Applicants = applicants;
            Modifiable = modifiable;
            ResultsGenerated = generatedResults;
            ExamineesNotified = examineesNotified;
            CreatedAt = createdAt;
        }

        public ExamSlot() { }

        public bool ApplicationsVisible()
        {
            int daysLeft = (TimeSlot.Time - DateTime.Now).Days; // days left until exam
            double timeLeft = (TimeSlot.GetEnd() - DateTime.Now).TotalMinutes; // time left until end of exam

            if (daysLeft > 0 && daysLeft < Constants.PRE_START_VIEW_PERIOD) return true; // applications become visible when there are less than PRE_START_VIEW_PERIOD days left
            else if (daysLeft == 0 && timeLeft > 0) return true; // on the exam day, applications are visible until the end of exam
            return false;
        }
        public bool IsHeldInLastYear()
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            return TimeSlot.Time > oneYearAgo;
        }
    }
}
