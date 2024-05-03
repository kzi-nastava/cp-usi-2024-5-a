using LangLang.Core.Model;

namespace LangLang.DTO
{
    public class TutorRatingDTO
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }

        public TutorRatingDTO() { }
        public TutorRatingDTO(TutorRating rating)
        {
            Id = rating.Id;
            TutorId = rating.TutorId;
            StudentId = rating.StudentId;
            Rating = rating.Rating;
        }

        public TutorRating ToTutorRating()
        {
            return new TutorRating(Id, TutorId, StudentId, Rating);
        }
    }
}
