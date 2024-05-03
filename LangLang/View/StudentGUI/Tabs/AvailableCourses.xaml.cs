using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI.Tabs
{
    public partial class AvailableCourses : UserControl
    {
        private readonly StudentWindow parentWindow;
        private readonly AppController appController;
        private readonly CourseController courseController;
        private readonly Student currentlyLoggedIn;
        public ObservableCollection<CourseDTO> Courses { get; set; }
        public List<Course> CoursesForReview {  get; set; }
        private EnrollmentRequestDTO EnrollmentRequest {  get; set; }
        public CourseDTO SelectedCourse { get; set; }

        public AvailableCourses(AppController appController, Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            DataContext = this;
            this.parentWindow = parentWindow;
            this.appController = appController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            courseController = appController.CourseController;
            Courses = new();
            EnrollmentRequest = new();
            SetDataForReview();
            levelCoursecb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            AdjustButton();
        }

        public void SetDataForReview()
        {
            var studentController = appController.StudentController;
            var enrollmentController = appController.EnrollmentRequestController;

            CoursesForReview = studentController.GetAvailableCourses(currentlyLoggedIn.Id, courseController, enrollmentController);
        }

        private void AdjustButton()
        {
            var studentController = appController.StudentController;
            var enrollmentController = appController.EnrollmentRequestController;
            var withdrawalController = appController.WithdrawalRequestController;
            if (!studentController.CanRequestEnroll(currentlyLoggedIn.Id, enrollmentController, courseController, withdrawalController))
                SendRequestBtn.IsEnabled = false;
        }


        private void SearchCourses(object sender, RoutedEventArgs e)
        {

            string? language = languagetb.Text;
            LanguageLevel? level = null;
            if (levelCoursecb.SelectedValue != null)
                level = (LanguageLevel)levelCoursecb.SelectedValue;
            DateTime courseStartDate = courseStartdp.SelectedDate ?? default;
            int.TryParse(durationtb.Text, out int duration);
           
            var studentController = appController.StudentController;
            var enrollmentController = appController.EnrollmentRequestController;
            CoursesForReview = studentController.SearchCoursesByStudent(currentlyLoggedIn.Id, courseController, enrollmentController, language, level, courseStartDate, duration, !onlinecb.IsChecked);
            parentWindow.Update();
        }

        private void ClearCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            var studentController = appController.StudentController;
            var enrollmentController = appController.EnrollmentRequestController;

            CoursesForReview = studentController.GetAvailableCourses(currentlyLoggedIn.Id, courseController, enrollmentController);
            levelCoursecb.SelectedItem = null;
            parentWindow.Update();
        }

        private void SendRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCourse == null) return;
            EnrollmentRequest.CourseId = SelectedCourse.Id;
            EnrollmentRequest.StudentId = currentlyLoggedIn.Id;
            EnrollmentRequest.Status = Status.Pending;
            EnrollmentRequest.RequestSentAt = DateTime.Now;
            EnrollmentRequest.LastModifiedTimestamp = DateTime.Now;
            EnrollmentRequest.IsCanceled = false;

            var enrollmentController = appController.EnrollmentRequestController;
            enrollmentController.Add(EnrollmentRequest.ToEnrollmentRequest());

            MessageBox.Show("Request sent. Please wait for approval.");

            var studentController = appController.StudentController;
            CoursesForReview = studentController.GetAvailableCourses(currentlyLoggedIn.Id, courseController, enrollmentController);
            parentWindow.enrollmentRequestsTab.SetDataForReview();
            parentWindow.Update();
        }

    }
}
