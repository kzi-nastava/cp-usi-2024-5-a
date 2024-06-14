using System;

namespace LangLang.Domain.Models
{
    public class ExamSlot
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int TutorId { get; set; }
        public int TimeSlotId { get; set; }
        public int MaxStudents { get; set; }
        public int Applicants { get; set; }
        public bool Modifiable { get; set; }
        public bool ResultsGenerated { get; set; }
        public bool ExamineesNotified { get; set; }
        public DateTime CreatedAt { get; set; }

        public ExamSlot(int id, int languageId, int timeSlotId, int maxStudents, int tutorId, int applicants, bool modifiable, bool generatedResults, bool examineesNotified, DateTime createdAt)
        {
            Id = id;
            LanguageId = languageId;
            TutorId = tutorId;
            TimeSlotId = timeSlotId;
            MaxStudents = maxStudents;
            Applicants = applicants;
            Modifiable = modifiable;
            ResultsGenerated = generatedResults;
            ExamineesNotified = examineesNotified;
            CreatedAt = createdAt;
        }

        public ExamSlot() { }

    }
}
