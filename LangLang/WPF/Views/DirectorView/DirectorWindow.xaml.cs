using System.Windows;
using System.Windows.Controls;
using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.Views.DirectorView.Tabs;

namespace LangLang.WPF.Views.DirectorView
{
    public partial class DirectorWindow : Window
    {
        public Director CurrentlyLoggedIn { get; set; }
        public DirectorWindow(Profile currentlyLoggedIn)
        {
            InitializeComponent();
            DataContext = this;
            var directorService = new DirectorService();
            CurrentlyLoggedIn = directorService.Get(currentlyLoggedIn.Id);
            GenerateTabs();
        }

        private void GenerateTabs()
        {
            var reviewTab = new TutorReview();
            AddTab("Tutor review", reviewTab);
            var resultsTab = new ResultsSending();
            AddTab("Results Sending", resultsTab);
            var gradedCoursesTab = new GradedCourses();
            AddTab("Graded courses", gradedCoursesTab);
            var reportsTab = new Reports(CurrentlyLoggedIn);
            AddTab("Reports", reportsTab);
            var coursesTab = new CoursesReview();
            AddTab("Courses", coursesTab);
            var examsTab = new ExamSlotsReview();
            AddTab("Exams", examsTab);
        }

        private void AddTab(string header, UserControl content)
        {
            TabItem tabItem = new()
            {
                Header = header,
                Content = content
            };
            tabControl.Items.Add(tabItem);
        }

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }

    }
}
