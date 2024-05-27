
namespace LangLang.Domain.Models
{
    public class TutorRating 
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TutorId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }

        public TutorRating() { }

        public TutorRating(int id, int courseId, int tutorId, int studentId, int rating)
        {
            Id = id;
            CourseId = courseId;
            TutorId = tutorId;
            StudentId = studentId;
            Rating = rating;
        }
    }
}
