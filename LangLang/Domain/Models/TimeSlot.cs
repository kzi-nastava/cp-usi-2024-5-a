using System;
using System.ComponentModel;
using LangLang.Configuration;
using LangLang.ConsoleApp.Attributes;

namespace LangLang.Domain.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        // NOTE: Adapt as needed during implementation
        [Show]
        [DisplayName("Duration")]
        [AllowCreate]
        public double Duration { get; set; }
        [Show]
        [DisplayName("Date and time")]
        [AllowCreate]
        public DateTime Time { get; set; }
        public TimeSlot() { }
        public TimeSlot(double duration, DateTime time)
        {   
            Duration = duration;
            Time = time;
        }

        public bool OverlappsWith(TimeSlot timeSlot)
        {
            DateTime end = Time.AddHours(Duration);
            DateTime otherEnd = timeSlot.Time.AddHours(timeSlot.Duration);

            return Time <= otherEnd && Time >= timeSlot.Time || end >= timeSlot.Time && end <= otherEnd;
        }

        public bool IsInFuture()
        {
            return Time > DateTime.Now;
        }

        public DateTime GetEnd()
        {
            return Time.AddHours(Duration);
        }
    }
}
