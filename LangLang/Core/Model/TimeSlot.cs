using LangLang.Core.Model.Enums;
using System;
using LangLang.Core.Repository.Serialization;

namespace LangLang.Core.Model
{
    public class TimeSlot
    {
        // NOTE: Adapt as needed during implementation
        public int Duration { get; set; }
        public DateTime Time { get; set; }

        public TimeSlot(int duration, DateTime time)
        {
            Duration = duration;
            Time = time;
        }

        public TimeSlot(string duration, string time)
        {
            try
            {
                Time = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm", null);
            }
            catch
            {
                throw new FormatException("Date is not in the correct format.");
            }
            Duration = int.Parse(duration);

        }

        public bool OverlappsWith(TimeSlot timeSlot)
        {
            DateTime end = Time.AddHours(Duration);
            DateTime otherEnd = timeSlot.Time.AddHours(timeSlot.Duration);

            return ((Time < otherEnd && Time > timeSlot.Time) || ( end > timeSlot.Time && end < otherEnd));
        }

        public bool IsInFuture()
        {
            return (Time > DateTime.Now);
        }

        

        public string ToString()
        {
            return Duration.ToString() + '|' + Time.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
