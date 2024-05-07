﻿using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using LangLang.DTO;
using LangLang.View.StudentGUI.Tabs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI
{
    public partial class StudentWindow : Window, IObserver
    {
        public StudentDTO Student { get; set; }
        private AppController appController;
        private Student currentlyLoggedIn;
        public AvailableCourses availableCoursesTab {  get; set; }
        public AvailableExams availableExamsTab {  get; set; }
        public ExamApplications examApplicationsTab { get; set; }
        public EnrollmentRequests enrollmentRequestsTab {  get; set; }

        public StudentWindow(AppController appController, Profile currentlyLoggedIn)
        {
            InitializeComponent();
            DataContext = this;
            this.appController = appController;

            var studentController = appController.StudentController;
            this.currentlyLoggedIn = studentController.Get(currentlyLoggedIn.Id);
            Student = new(this.currentlyLoggedIn);

            GenerateTabs();
            Update();
        }

        private void GenerateTabs()
        {
            StudentData studentDataTab = new(appController, currentlyLoggedIn, this);
            AddTab("Student data", studentDataTab);
            ActiveCourse activeCourseTab = new(appController, currentlyLoggedIn, this);
            AddTab("Active course", activeCourseTab);
            availableCoursesTab = new(appController, currentlyLoggedIn, this);
            AddTab("Available courses", availableCoursesTab);
            availableExamsTab = new(appController, currentlyLoggedIn, this);
            AddTab("Available exams", availableExamsTab);
            examApplicationsTab = new(appController, currentlyLoggedIn, this);
            AddTab("Exam applications", examApplicationsTab);
            CompletedCourses completedCoursesTab = new(appController, currentlyLoggedIn, this);
            AddTab("Completed courses", completedCoursesTab);
            enrollmentRequestsTab = new(appController, currentlyLoggedIn, this);
            AddTab("Course enrollment requests", enrollmentRequestsTab);
            Notifications notificationsTab = new(appController, currentlyLoggedIn, this);
            AddTab("Notifications", notificationsTab);
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


        public void Update()
        {   
            availableCoursesTab.Courses.Clear();
            foreach (var course in availableCoursesTab.CoursesForReview)
                availableCoursesTab.Courses.Add(new CourseDTO(course));

            availableExamsTab.ExamSlots.Clear();
            foreach (var exam in availableExamsTab.ExamsForReview)
                availableExamsTab.ExamSlots.Add(new ExamSlotDTO(exam));

            examApplicationsTab.Applications.Clear();
            foreach (var app in examApplicationsTab.ApplicationsForReview)
                examApplicationsTab.Applications.Add(new ExamApplicationDTO(app,appController));

            enrollmentRequestsTab.StudentRequests.Clear();
            foreach (EnrollmentRequest er in enrollmentRequestsTab.RequestsForReview)
                enrollmentRequestsTab.StudentRequests.Add(new EnrollmentRequestDTO(er, appController));

        }

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }
    }
}
