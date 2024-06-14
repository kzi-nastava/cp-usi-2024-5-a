using System;
using System.ComponentModel;
using LangLang.Configuration;
using LangLang.ConsoleApp.Attributes;

namespace LangLang.Domain.Models
{
    public class TimeSlot
    {
        // NOTE: Adapt as needed during implementation
        [Show]
        [DisplayName("Duration")]
        public double Duration { get; set; }
        [Show]
        [DisplayName("Date and time")]
        public DateTime Time { get; set; }
        public TimeSlot() { }
        public TimeSlot(double duration, DateTime time)
        {
            Duration = duration;
            Time = time;
        }

        public TimeSlot(string duration, string time)
        {
            try
            {
                Time = DateTime.ParseExact(time, Constants.DATE_TIME_FORMAT, null);
            }
            catch
            {
                throw new FormatException("Date is not in the correct format.");
            }
            Duration = double.Parse(duration);

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

        public string ToString()
        {
            return Duration.ToString() + '|' + Time.ToString(Constants.DATE_TIME_FORMAT);
        }

        public DateTime GetEnd()
        {
            return Time.AddHours(Duration);
        }
    }
}
