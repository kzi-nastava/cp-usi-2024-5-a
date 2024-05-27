using System;

namespace LangLang.Domain.Models
{
    public class PenaltyPoint
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TutorId { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }

        public PenaltyPoint() { }
        public PenaltyPoint(int id, int studentId, int tutorId, int courseId, DateTime date)
        {
            Id = id;
            StudentId = studentId;
            TutorId = tutorId;
            CourseId = courseId;
            Date = date;
        }

    }
}
