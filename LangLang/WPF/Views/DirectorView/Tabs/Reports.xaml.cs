using LangLang.WPF.ViewModels.DirectorViewModels;
using System.Windows.Controls;
using LangLang.Domain.Models;

namespace LangLang.WPF.Views.DirectorView.Tabs
{
    public partial class Reports : UserControl
    {
        private ReportsViewModel _viewModel;
        public Reports(Director loggedIn)
        {
            InitializeComponent();
            _viewModel = new(loggedIn);
            DataContext = _viewModel;
        }

        private void AveragePoints_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SentAveragePoints();
        }

        private void AveragePenaltyPoints_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SentAveragePenaltyPoints();
        }

        private void AverageGradeByPenaltyCount_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SendAverageGradeByPenaltyCount();
        }

        private void PenaltiesCountLastYear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SendPenaltiesCountLastYear();
        }

        private void AverageCourseGrades_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SentAverageCourseGrades();
        }

        private void AverageResultsPerSkill_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SentAverageResultsPerSkill();
        }

        private void CoursesAccomplishments_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SentCoursesAccomplishments();
        }

        private void CoursesCreated_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SentCoursesCreated();
        }

        private void ExamsCreated_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SentExamsCreated();
        }

    }
}
