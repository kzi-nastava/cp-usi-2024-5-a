

using LangLang.BusinessLogic.UseCases;
using System.Windows;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class TutorRatingPageViewModel
    {
        private TutorRatingViewModel tutorRating { get; set; }
        public TutorRatingPageViewModel(TutorRatingViewModel tutorRating) {
            this.tutorRating = tutorRating;
        }

        public void ChangeRate(int selectedRate)
        {
            tutorRating.Rating = selectedRate;
        }

        public void RateTutor()
        {
            var service = new TutorRatingService();
            service.Add(tutorRating.ToTutorRating());
            MessageBox.Show("Thank you for your feedback!", "Feedback Submitted");

        }
    }
}
