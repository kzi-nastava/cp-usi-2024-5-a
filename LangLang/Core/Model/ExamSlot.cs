﻿using LangLang.Core.Repository.Serialization;
using System;
using System.Diagnostics.Eventing.Reader;

namespace LangLang.Core.Model
{
    public class ExamSlot: ISerializable
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public LanguageLevel Level { get; set; }
        public int TutorId { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int MaxStudents { get; set; }
        public bool Modifiable { get; set; }

        // NOTE: if possible don't save number of registeredStudents, ask the database. If not, then add attribute.
        

        public ExamSlot(int id, string language, LanguageLevel level, TimeSlot timeSlot, int maxStudents, int tutorId, bool modifiable)
        {
            Id = id;
            Language = language;
            Level = Level;
            TutorId = tutorId;
            TimeSlot = timeSlot;
            MaxStudents = maxStudents;
            Modifiable = modifiable;
        }

        public ExamSlot() { }

        public string[] ToCSV()
        {
            return new string[] {
            Id.ToString(),
            Language,
            Level.ToString(),
            TutorId.ToString(),
            TimeSlot.ToString(),
            MaxStudents.ToString(),
            Modifiable.ToString()
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Language = values[1];
            Level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), values[2]);
            TutorId = int.Parse(values[3]);
            TimeSlot = new (values[4], values[5]);
            MaxStudents = int.Parse(values[6]);
            Modifiable = bool.Parse(values[7]);
        }

        public bool ApplicationsVisible()
        {
            int daysLeft = (TimeSlot.Time - DateTime.Now).Days; // days left until exam
            double timeLeft = (TimeSlot.GetEnd() - DateTime.Now).TotalMinutes; // time left until end of exam

            if (daysLeft > 0 && daysLeft < 7) return true; // seven days before, applications are visible
            else if (daysLeft == 0 && timeLeft > 0) return true; // on the exam day, applications are visible until the end of exam
            return false;
        }

    }
}
