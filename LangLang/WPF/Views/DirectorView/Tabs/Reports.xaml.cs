using LangLang.WPF.ViewModels.DirectorViewModels;
using System.Windows.Controls;

namespace LangLang.WPF.Views.DirectorView.Tabs
{
    // TODO: Add a click listener to the button for the corresponding report
    // NOTE: The logic should be implemented in ReportsViewModel.cs

    public partial class Reports : UserControl
    {
        private ReportsViewModel _viewModel;
        public Reports()
        {
            InitializeComponent();
            _viewModel = new();
            DataContext = _viewModel;
        }

    }
}
