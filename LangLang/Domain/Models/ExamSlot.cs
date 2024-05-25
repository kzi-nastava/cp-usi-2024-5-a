using LangLang.Configuration;
using LangLang.Core.Model;
using LangLang.Domain.Enums;
using System;

namespace LangLang.Domain.Models
{
    public class ExamSlot
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public LanguageLevel Level { get; set; }
        public int TutorId { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int MaxStudents { get; set; }
        public int Applicants { get; set; }
        public bool Modifiable { get; set; }
        public bool ResultsGenerated { get; set; }

        public ExamSlot(int id, string language, LanguageLevel level, TimeSlot timeSlot, int maxStudents, int tutorId, int applicants, bool modifiable, bool generatedResults)
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

    }
}
