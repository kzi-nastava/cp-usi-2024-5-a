using LangLang.WPF.ViewModels.ExamViewModels;
using System.Windows;
using System.Windows.Controls;


namespace LangLang.WPF.Views.TutorView.AdditionalWindows.ExamSlotView
{
    public partial class EnterResults : Window
    {
        public EnterResultsViewModel EnterResultsViewModel { get; set; }

        public EnterResults(ExamSlotViewModel selectedExam)
        {
            InitializeComponent();
            EnterResultsViewModel = new(selectedExam);
            DataContext = EnterResultsViewModel;

            DisableForm();
            Update();
        }

        private void Update()
        {
            EnterResultsViewModel.Update();
        }

        private void ExamResultDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EnterResultsViewModel.SelectedResult != null)
                EnableForm();
            else
                DisableForm();
        }

        private void confirmResultBtn_Click(object sender, RoutedEventArgs e)
        {
            EnterResultsViewModel.ConfirmResult();
        }

        private void DisableForm()
        {
            nameTB.IsEnabled = false;
            lastnameTB.IsEnabled = false;
            emailTB.IsEnabled = false;
            readingPointsTB.IsEnabled = false;
            listeningPointsTB.IsEnabled = false;
            writingPointsTB.IsEnabled = false;
            speakingPointsTB.IsEnabled = false;
            confirmResultBtn.IsEnabled = false;
        }

        private void EnableForm()
        {
            readingPointsTB.IsEnabled = true;
            listeningPointsTB.IsEnabled = true;
            writingPointsTB.IsEnabled = true;
            speakingPointsTB.IsEnabled = true;
            confirmResultBtn.IsEnabled = true;
        }

    }
}

