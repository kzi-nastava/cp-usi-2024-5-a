using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.WPF.Views;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI.Tabs
{
    public partial class ActiveCourse : UserControl
    {
        private StudentWindow parentWindow { get; set; }
        private AppController appController { get; set; }
        private Student currentlyLoggedIn { get; set; }
        private int acceptedRequestId { get; set; }
        public ActiveCourse(AppController appController, Student currentlyLoggedIn, StudentWindow studentWindow)
        {
            this.appController = appController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            InitializeComponent();
            FillCourseInfo();
            parentWindow = studentWindow;
            AdjustButton();
        }

        private void FillCourseInfo()
        {
            var enrollmentController = appController.EnrollmentRequestController;
            var courseController = appController.CourseController;

            var enrollmentRequest = enrollmentController.GetActiveCourseRequest(currentlyLoggedIn, appController);
            if (enrollmentRequest == null)
            {
                HideWithdrawalBtn();
                return;
            }

            acceptedRequestId = enrollmentRequest.Id;
            Course activeCourse = courseController.Get(enrollmentRequest.CourseId);
            courseNameTb.Text = activeCourse.Language;
            courseLevelTb.Text = activeCourse.Level.ToString();
            string daysUntilEnd = activeCourse.DaysUntilEnd().ToString();
            untilEndTb.Text = daysUntilEnd + " days until the end of course.";
        }

        private void HideWithdrawalBtn()
        {
            untilEndTb.Text = "You are currently not enrolled in any courses. " +
                            "\nYou can request enrollment or wait for the tutor to accept your request.";
            CourseWithdrawalBtn.Visibility = Visibility.Collapsed;
        }
        private void CourseWithdrawalBtn_Click(object sender, RoutedEventArgs e)
        {
            var withdrawalController = appController.WithdrawalRequestController;
            if (withdrawalController.AlreadyExists(acceptedRequestId))
            {
                MessageBox.Show("Request already submitted. Wait for response.");
                return;
            }
            WithdrawalRequestWindow wrWindow = new(appController, acceptedRequestId, parentWindow);
            wrWindow.Show();
        }

        private void AdjustButton()
        {
            var enrollmentController = appController.EnrollmentRequestController;
            var withdrawalController = appController.WithdrawalRequestController;
            if (CourseWithdrawalBtn.Visibility == Visibility.Visible)
            {
                // disable the withdrawal request button if the student is ineligible to withdraw or has already withdrawn
                if (!enrollmentController.CanRequestWithdrawal(acceptedRequestId) || withdrawalController.AlreadyExists(acceptedRequestId))
                    CourseWithdrawalBtn.IsEnabled = false;
            }
        }

    }
}
