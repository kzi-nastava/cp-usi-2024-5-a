using LangLang.Configuration;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.Views.DirectorView.AdditionalWindows;
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

        private void CoursesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CoursesVM.SelectedCourse.TutorId == Constants.DELETED_TUTOR_ID)
            {
                tutorBtn.IsEnabled = false;
            }
            else
            {
                tutorBtn.IsEnabled = true;
            }
        }

        public void Update()
        {
            CoursesVM.Update();
        }
    }
}
