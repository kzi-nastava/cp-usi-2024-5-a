using LangLang.WPF.ViewModels.CourseViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.DirectorView.Tabs
{
    public partial class GradedCourses : UserControl
    {
        private GradedCourseViewModel viewModel;
        public GradedCourses()
        {
            InitializeComponent();
            viewModel = new();
            DataContext = viewModel;
            startSmartSystem.IsEnabled = false;
        }

        private void CompletedCoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.SelectedCourse != null) startSmartSystem.IsEnabled = true;
            else startSmartSystem.IsEnabled = false;
        }

        private void startSmartSystem_Click(object sender, RoutedEventArgs e)
        {
            viewModel.StartSmartSystem(knowledge.IsChecked == true);
        }
    }
}
