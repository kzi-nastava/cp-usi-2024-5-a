using LangLang.Core.Controller;
using LangLang.DTO;
using System.Windows;

namespace LangLang.View.StudentGUI
{

    public partial class TutorRating : Window
    {
        private TutorRatingController controller { get; set; }
        private TutorRatingDTO tutorRating {  get; set; }
        public TutorRating(AppController appController, TutorRatingDTO tutorRating, string tutorFullName)
        {
            this.tutorRating = tutorRating;
            InitializeComponent();
            controller = appController.TutorRatingController;
            fullNameTextBlock.Text = tutorFullName;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int selectedRate = (int)slider.Value;
            tutorRating.Rating = selectedRate;
            rateTextBlock.Text = selectedRate.ToString();
        }

        private void RateBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.Add(tutorRating.ToTutorRating());
            MessageBox.Show("Thank you for your feedback!", "Feedback Submitted");
            Close();
        }
    }
}
