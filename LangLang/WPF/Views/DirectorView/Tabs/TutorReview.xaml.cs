using LangLang.WPF.ViewModels.TutorViewModels;
using LangLang.WPF.Views.DirectorView.AdditionalWindows;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.DirectorView.Tabs
{
    public partial class TutorReview : UserControl
    {
        public TutorReviewPageViewModel TutorReviewViewModel { get; set; }

        public TutorReview()
        {
            InitializeComponent();
            TutorReviewViewModel = new();
            DataContext = TutorReviewViewModel;
            DisableButtons();
            Update();
        }

        private void AddTutor_Click(object sender, RoutedEventArgs e)
        {
            AddTutorWindow window = new(this);
            window.Show();
        }

        private void UpdateTutor_Click(object sender, RoutedEventArgs e)
        {
            TutorViewModel tutor = new(TutorReviewViewModel.SelectedTutor);
            UpdateTutor window = new(tutor, this);
            window.Show();
        }
            private void DeleteTutor_Click(object sender, RoutedEventArgs e)
        {
            TutorReviewViewModel.DeleteTutor();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            TutorReviewViewModel.Search();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            TutorReviewViewModel.Update();
        }

        private void TutorsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TutorReviewViewModel.SelectedTutor == null)
                DisableButtons();
            else
                EnableButtons();
        }

        private void EnableButtons()
        {
            updateBtn.IsEnabled = true;
            deleteBtn.IsEnabled = true;
        }

        private void DisableButtons()
        {
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

        public void Update()
        {
            TutorReviewViewModel.Update();
        }
    }
}
