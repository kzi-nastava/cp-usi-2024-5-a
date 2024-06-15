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
            bool online, int numberOfStudents, int maxStudents, DateTime startDateTime, bool createdByDirector, bool modifiable, bool gratitudeEmailSent, DateTime createdAt)
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

        public bool IsCompleted()
        {
            // TODO: uncomment and fix
            //TimeSlot timeSlot = TimeSlots[TimeSlots.Count - 1];
            //return DateTime.Now >= timeSlot.GetEnd();
            return false;
        }

        // this method checks if timeSlot overlapps with any of the course's timeslots
        public bool OverlappsWith(TimeSlot timeSlot)
        {
            // TODO: uncomment and fix
            //foreach (TimeSlot time in TimeSlots)
            //{
            //    if (time.OverlappsWith(timeSlot))
            //    {
            //        return true;
            //    }
            //}
            return false;
        }
        public int DaysUntilEnd()
        {
            // TODO: uncomment and fix
            //var endDate = TimeSlots[^1].GetEnd();
            //return (endDate - DateTime.Now).Days;
            return 0;
        }

        public int DaysUntilStart()
        {
            return (StartDateTime - DateTime.Now).Days;
        }

        public bool IsHeldInLastYear()
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            return GetEnd() > oneYearAgo && GetEnd() <= DateTime.Now;
        }

        public string ToPdfString()
        {
            // TODO: uncomment
            //return Id + " " + " " + Language + " " + Level;
            return "";
        }

        public DateTime GetEnd()
        {
            // TODO: fix
            //return TimeSlots[TimeSlots.Count - 1].GetEnd();
            return DateTime.Now;
        }
        public bool IsActive()
        {
            if (StartDateTime <= DateTime.Now && GetEnd() >= DateTime.Now) return true;
            return false;
        }
        public bool CanChange()
        {
            return StartDateTime >= DateTime.Now.AddDays(Constants.COURSE_MODIFY_PERIOD); ;
        }
    }
}
