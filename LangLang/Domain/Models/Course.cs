using LangLang.Configuration;
using System;
using System.Collections.Generic;

namespace LangLang.Domain.Models
{
    public class Course
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int LanguageLevelId { get; set; }
        public int NumberOfWeeks { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public bool Online { get; set; }
        public int NumberOfStudents { get; set; }
        public int MaxStudents { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool CreatedByDirector { get; set; }
        public bool Modifiable { get; set; }
        public bool GratitudeEmailSent { get; set; }
        public DateTime CreatedAt { get; set; }

        public Course(int id, int tutorId, int languageLevelId, int numberOfWeeks, List<DayOfWeek> days,
            bool online, int numberOfStudents, int maxStudents, DateTime startDateTime, bool createdByDirector, 
            bool modifiable, bool gratitudeEmailSent, DateTime createdAt)
        {
            Id = id;
            TutorId = tutorId;
            LanguageLevelId = languageLevelId;
            NumberOfWeeks = numberOfWeeks;
            Days = days;
            Online = online;
            NumberOfStudents = numberOfStudents;
            MaxStudents = maxStudents;
            StartDateTime = startDateTime;
            CreatedByDirector = createdByDirector;
            Modifiable = modifiable;
            GratitudeEmailSent = gratitudeEmailSent;
            CreatedAt = createdAt;
        }

        public Course() { }

        public int DaysUntilStart()
        {
            return (StartDateTime - DateTime.Now).Days;
        }

        public bool CanChange()
        {
            return StartDateTime >= DateTime.Now.AddDays(Constants.COURSE_MODIFY_PERIOD); ;
        }
    }
}
