using LangLang.Domain.Models;
using Org.BouncyCastle.Asn1.Mozilla;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class TutorRatingViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TutorId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }

        public TutorRatingViewModel() { }
        public TutorRatingViewModel(TutorRating rating)
        {
            Id = rating.Id;
            CourseId = rating.CourseId;
            TutorId = rating.TutorId;
            StudentId = rating.StudentId;
            Rating = rating.Rating;
        }

        public TutorRating ToTutorRating()
        {
            return new TutorRating(Id, CourseId, TutorId, StudentId, Rating);
        }
    }
}
