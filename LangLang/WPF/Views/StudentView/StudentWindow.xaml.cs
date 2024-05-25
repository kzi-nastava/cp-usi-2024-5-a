using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using LangLang.WPF.Views.StudentView.Tabs;
using System.Windows;
using System.Windows.Controls;
using LangLang.WPF.ViewModels.StudentViewModels;
using LangLang.View.StudentGUI.Tabs;

namespace LangLang.WPF.Views.StudentView
{
    public partial class StudentWindow : Window
    {
        private Student CurrentlyLoggedIn;
        public AvailableCourses AvailableCoursesTab { get; set; }
        public AvailableExams AvailableExamsTab { get; set; }
        public ExamApplications ExamApplicationsTab { get; set; }
        public EnrollmentRequests EnrollmentRequestsTab { get; set; }
        public StudentWindowViewModel ViewModel { get; set; }

        public StudentWindow(Profile currentlyLoggedIn)
        {
            InitializeComponent();

            var studentService = new StudentService();
            CurrentlyLoggedIn = studentService.Get(currentlyLoggedIn.Id);

            ViewModel = new StudentWindowViewModel();
            DataContext = ViewModel;

            GenerateTabs(); 
            Update();
        }

        public void Update()
        {
            ViewModel.UpdateAvailableCourses(AvailableCoursesTab);
            ViewModel.UpdateAvailableExams(AvailableExamsTab);
            ViewModel.UpdateEnrollmentRequests(EnrollmentRequestsTab);
            ViewModel.UpdateExamApplications(ExamApplicationsTab);
        }

        private void GenerateTabs()
        {
            AddTab("Student data", new DataTab(CurrentlyLoggedIn, this));
            
            AddTab("Active course", new ActiveCourseTab(CurrentlyLoggedIn));
            
            AvailableCoursesTab = new(CurrentlyLoggedIn, this);
            AddTab("Available courses", AvailableCoursesTab);
            
            AvailableExamsTab = new(CurrentlyLoggedIn, this);
            AddTab("Available exams", AvailableExamsTab);
            
            ExamApplicationsTab = new(CurrentlyLoggedIn, this);
            AddTab("Exam applications", ExamApplicationsTab);
            
            EnrollmentRequestsTab = new(CurrentlyLoggedIn, this);
            AddTab("Course enrollment requests", EnrollmentRequestsTab);
            
            AddTab("Completed courses", new CompletedCourses(CurrentlyLoggedIn));
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
