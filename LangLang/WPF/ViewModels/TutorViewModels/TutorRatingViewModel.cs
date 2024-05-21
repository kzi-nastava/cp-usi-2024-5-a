using LangLang.Domain.Models;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class TutorRatingViewModel
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }

        public TutorRatingViewModel() { }
        public TutorRatingViewModel(TutorRating rating)
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
