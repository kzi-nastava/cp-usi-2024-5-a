using LangLang.Core.Repository.Serialization;

namespace LangLang.Domain.Models
{
    public class TutorRating 
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }

        public TutorRating() { }

        public TutorRating(int id, int tutorId, int studentId, int rating)
        {
            Id = id;
            TutorId = tutorId;
            StudentId = studentId;
            Rating = rating;
        }
    }
}
