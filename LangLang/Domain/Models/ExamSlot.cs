using System;
﻿using LangLang.Configuration;
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
        [AllowCreate]
        public int LanguageId { get; set; }
        [Show]
        [AllowUpdate]
        [DisplayName("Exam time")]
        [AllowCreate]
        public int TimeSlotId { get; set; }

        [Show]
        [DisplayName("Tutor")]
        public int TutorId { get; set; }
        
        [Show]
        [AllowUpdate]
        [DisplayName("Max Students")]
        [AllowCreate]
        public int MaxStudents { get; set; }
        [Show]
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
