using LangLang.Configuration;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.Views.DirectorView.AdditionalWindows;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.DirectorView.Tabs
{
    /// <summary>
    /// Interaction logic for Courses.xaml
    /// </summary>
    public partial class CoursesReview : UserControl
    {
        public CoursesDirectorVM CoursesVM { get; set; }
        public CoursesReview()
        {
            InitializeComponent();
            CoursesVM = new();
            DataContext = CoursesVM;
            tutorBtn.IsEnabled = false;
            CoursesVM.Update();
        }

        
        private void CourseCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseCreateWindow createWindow = new(this);
            createWindow.Show();
        }
        private void AssignTutorBtn_Click(object sender, RoutedEventArgs e)
        {
            CoursesVM.AssignTutor(CoursesVM.SelectedCourse);
        }

        private void CoursesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!CoursesVM.HasTutor())
                tutorBtn.IsEnabled = true;
            else
                tutorBtn.IsEnabled = false;
            
        }

        public void Update()
        {
            tutorBtn.IsEnabled = false;
            CoursesVM.Update();
        }
    }
}
