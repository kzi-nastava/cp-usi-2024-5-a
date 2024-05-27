using LangLang.WPF.ViewModels.DirectorViewModels;
using System.Windows.Controls;
using LangLang.Domain.Models;

namespace LangLang.WPF.Views.DirectorView.Tabs
{
    // TODO: Add a click listener to the button for the corresponding report
    // NOTE: The logic should be implemented in ReportsViewModel.cs

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
    }
}
