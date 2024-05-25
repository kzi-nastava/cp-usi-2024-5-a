using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI.Tabs
{
    public partial class CompletedCourses : UserControl
    {
        
        public CompletedCourseViewModel completedCourseVM { get; set; }
        public CompletedCourses(Student currentlyLoggedIn)
        {
            InitializeComponent();
            completedCourseVM = new(currentlyLoggedIn);
            DataContext = completedCourseVM;
            SetDataForReview();
            rateTutorBtn.IsEnabled = false;
        }

        private void SetDataForReview()
        {
            completedCourseVM.SetDataForReview();
        }

        private void CompletedCoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (completedCourseVM.ToEnableButton()) rateTutorBtn.IsEnabled = false;
            else rateTutorBtn.IsEnabled = true;
        }


        private void rateTutorBtn_Click(object sender, RoutedEventArgs e)
        {
            completedCourseVM.TryRateTutor();
        }
    }
}
