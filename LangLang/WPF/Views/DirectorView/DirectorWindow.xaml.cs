using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.DirectorViewModels;
using LangLang.WPF.Views.DirectorView.Tabs;
using LangLang.WPF.Views.StudentView.Tabs;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.DirectorView
{
    public partial class DirectorWindow : Window
    {
        public Director CurrentlyLoggedIn { get; set; }
        public CoursesReview coursesTab { get; set; }
        public ExamSlotsReview examsTab { get; set; }
        public DirectorWindowViewModel ViewModel { get; set; }

        public DirectorWindow(Profile currentlyLoggedIn)
        {
            InitializeComponent();
            ViewModel = new DirectorWindowViewModel();
            DataContext = ViewModel;
            var directorService = new DirectorService();
            CurrentlyLoggedIn = directorService.Get(currentlyLoggedIn.Id);
            GenerateTabs();
        }

        private void GenerateTabs()
        {
            var reviewTab = new TutorReview(this);
            AddTab("Tutor review", reviewTab);
            var resultsTab = new ResultsSending();
            AddTab("Results Sending", resultsTab);
            var gradedCoursesTab = new GradedCourses();
            AddTab("Graded courses", gradedCoursesTab);
            var reportsTab = new Reports(CurrentlyLoggedIn);
            AddTab("Reports", reportsTab);
            coursesTab = new CoursesReview();
            AddTab("Courses", coursesTab);
            examsTab = new ExamSlotsReview();
            AddTab("Exams", examsTab);
        }

        public void Update()
        {
            ViewModel.UpdateCourses(coursesTab);
            ViewModel.UpdateExams(examsTab);
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
