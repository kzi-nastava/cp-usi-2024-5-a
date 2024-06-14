using System;

namespace LangLang.Domain.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public double Duration { get; set; }
        public DateTime Time { get; set; }

        public TimeSlot(int id, double duration, DateTime time)
        {   
            Id = id;
            Duration = duration;
            Time = time;
        }
        // NOTE: Possibly redudant
        //public TimeSlot(string duration, string time)
        //{
        //    try
        //    {
        //        Time = DateTime.ParseExact(time, Constants.DATE_TIME_FORMAT, null);
        //    }
        //    catch
        //    {
        //        throw new FormatException("Date is not in the correct format.");
        //    }
        //    Duration = double.Parse(duration);
        //}

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
