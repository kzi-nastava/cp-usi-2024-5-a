using LangLang.DTO;
using LangLang.WPF.ViewModels.TutorViewModels;
using System.Windows;

namespace LangLang.WPF.Views.StudentView
{

    public partial class TutorRatingWindow : Window
    {
        public TutorRatingPageViewModel TutorRatingViewModel { get; set; }
        public TutorRatingWindow(TutorRatingDTO tutorRating, string tutorFullName)
        {
            TutorRatingViewModel = new(tutorRating);
            InitializeComponent();
            fullNameTextBlock.Text = tutorFullName;
        }
        
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int selectedRate = (int)slider.Value;
            TutorRatingViewModel.ChangeRate(selectedRate);
            rateTextBlock.Text = selectedRate.ToString();
        }

        private void RateBtn_Click(object sender, RoutedEventArgs e)
        {
            TutorRatingViewModel.RateTutor();
            Close();
        }
    }
}
