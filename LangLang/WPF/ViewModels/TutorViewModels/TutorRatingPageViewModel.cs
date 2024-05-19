

using LangLang.Core.Controller;
using LangLang.DTO;
using System.Windows;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class TutorRatingPageViewModel
    {
        private TutorRatingDTO tutorRating { get; set; }
        public TutorRatingPageViewModel(TutorRatingDTO tutorRating) {
            this.tutorRating = tutorRating;
        }

        public void ChangeRate(int selectedRate)
        {
            tutorRating.Rating = selectedRate;
        }

        public void RateTutor()
        {
            var service = new TutorRatingController();
            service.Add(tutorRating.ToTutorRating());
            MessageBox.Show("Thank you for your feedback!", "Feedback Submitted");

        }
    }
}
