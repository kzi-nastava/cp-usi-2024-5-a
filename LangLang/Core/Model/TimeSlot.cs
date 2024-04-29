using System;
using System.Runtime.Serialization;

namespace LangLang.Core.Model
{
    public class TimeSlot
    {
        // NOTE: Adapt as needed during implementation
        public int Duration { get; set; }
        public DateTime time { get; set; }

        public bool OverlappsWith(TimeSlot timeSlot)
        {
            // TODO: Implement
            return false;
        }

        public bool IsInFuture()
        {
            // TODO: Implement
            return false;
        }

        // TODO: Add serialization methods
    }
}
