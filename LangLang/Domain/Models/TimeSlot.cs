using System;

namespace LangLang.Domain.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public double Duration { get; set; }
        public DateTime Time { get; set; }

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
