using LangLang.Aplication.UseCases;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using LangLang.WPF.Views.StudentView.Tabs;
using System.Windows;
using System.Windows.Controls;
using LangLang.WPF.ViewModels.StudentViewModels;

namespace LangLang.WPF.Views
{
    public partial class StudentWindow : Window, IObserver
    {
        private Student currentlyLoggedIn;
        //public AvailableCourses availableCoursesTab { get; set; }
        //public AvailableExams availableExamsTab { get; set; }
        //public ExamApplications examApplicationsTab { get; set; }
        //public EnrollmentRequests enrollmentRequestsTab { get; set; }
        public StudentWindowViewModel ViewModel { get; set; }

        public StudentWindow(Profile currentlyLoggedIn)
        {
            InitializeComponent();

            var studentService = new StudentService();
            this.currentlyLoggedIn = studentService.Get(currentlyLoggedIn.Id);

            ViewModel = new StudentWindowViewModel();
            DataContext = ViewModel;

            GenerateTabs();
        }

        public void Update()
        {
            ViewModel.UpdateData();
        }

        private void GenerateTabs()
        {
            AddTab("Student data", new DataTab(currentlyLoggedIn, this));
            //AddTab("Active course", new ActiveCourseTab(currentlyLoggedIn, this));
            //availableCoursesTab = new(currentlyLoggedIn, this);
            //AddTab("Available courses", availableCoursesTab);
            //availableExamsTab = new(currentlyLoggedIn, this);
            //AddTab("Available exams", availableExamsTab);
            //examApplicationsTab = new(currentlyLoggedIn, this);
            //AddTab("Exam applications", examApplicationsTab);
            ////CompletedCourses completedCoursesTab = new(currentlyLoggedIn, this);
            //AddTab("Completed courses", new CompletedCourses(currentlyLoggedIn, this));
            //enrollmentRequestsTab = new(currentlyLoggedIn, this);
            //AddTab("Course enrollment requests", enrollmentRequestsTab);
            //Notifications notificationsTab = new(currentlyLoggedIn, this);
            //AddTab("Notifications", notificationsTab);
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
