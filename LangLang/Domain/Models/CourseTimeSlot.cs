namespace LangLang.Domain.Models
{
    public class CourseTimeSlot
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TimeSlotId { get; set; }
        
        public CourseTimeSlot(int courseId, int timeSlotId)
        {
            CourseId = courseId;
            TimeSlotId = timeSlotId;
        }

    }
}
