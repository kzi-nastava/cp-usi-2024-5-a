using LangLang.Configuration;
using System;
using System.Collections.Generic;
using LangLang.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using LangLang.Domain.Attributes;

namespace LangLang.Domain.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Show]
        public int TutorId { get; set; }
        [Show]
        [AllowCreate]
        [AllowUpdate]
        public int LanguageLevelId { get; set; }

        [Show]
        [DisplayName("Weeks")]
        [AllowUpdate]
        [AllowCreate]
        public int NumberOfWeeks { get; set; }
        [Show]
        [AllowUpdate]
        [AllowCreate]
        [CourseDays]
        [DisplayName("Days")]
        public List<DayOfWeek> Days { get; set; }
        [Show]
        [AllowCreate]
        [AllowUpdate]
        public bool Online { get; set; }
        [Show]
        [DisplayName("Students")]
        public int NumberOfStudents { get; set; }
        [AllowUpdate]
        [AllowCreate]
        public int MaxStudents { get; set; }
        [Show]
        [DisplayName("Start Date and Time")]
        [AllowUpdate]
        [AllowCreate]
        public DateTime StartDateTime { get; set; }
        public bool CreatedByDirector { get; set; }
        public bool Modifiable { get; set; }
        public bool GratitudeEmailSent { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }

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

        public Course()
        {
        }

        public bool IsCompleted()
        {
            TimeSlot timeSlot = TimeSlots[TimeSlots.Count - 1];
            return DateTime.Now >= timeSlot.GetEnd();
        }

        // this method generates all timeslots for a course based on number of weeks, days and start datetime
        public void GenerateTimeSlots()
        {
            TimeSlots = new List<TimeSlot>();
            int skipToNextWeek = 0;
            for (int week = 0; week < NumberOfWeeks; week++)
            {
                foreach (DayOfWeek day in Days)
                {
                    // if the start date is after some of the days,
                    // skipped over to them in next week
                    if (day - StartDateTime.DayOfWeek < 0)
                    {
                        skipToNextWeek = 7;
                    }
                    else
                    {
                        skipToNextWeek = 0;
                    }
                    DateTime classDate = StartDateTime.AddDays(week * 7 + (skipToNextWeek + day - StartDateTime.DayOfWeek));
                    TimeSlots.Add(new TimeSlot(Constants.SESSION_DURATION, classDate));
                }
            }
        }

        // this method checks if timeSlot overlapps with any of the course's timeslots
        public bool OverlappsWith(TimeSlot timeSlot)
        {
            foreach (TimeSlot time in TimeSlots)
            {
                if (time.OverlappsWith(timeSlot))
                {
                    return true;
                }
            }
            return false;
        }
        public int DaysUntilEnd()
        {
            var endDate = TimeSlots[^1].GetEnd();
            return (endDate - DateTime.Now).Days;
        }

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
