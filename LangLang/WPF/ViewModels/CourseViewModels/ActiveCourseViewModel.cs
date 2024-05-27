using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.Views.StudentView.AdditionalWindows;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class ActiveCourseViewModel
    {
        public CourseViewModel Course { get; set; } 
        private Student currentlyLoggedIn;
        private int acceptedRequestId;
        public ActiveCourseViewModel(Student currentlyLoggedIn)
        {
            this.currentlyLoggedIn = currentlyLoggedIn;
        }

        public bool SetCourse()
        {
            var enrollmentService = new EnrollmentRequestService();
            var courseService = new CourseService();

            var enrollmentRequest = enrollmentService.GetActiveCourseRequest(currentlyLoggedIn);
            if (enrollmentRequest == null)
            {
                return false;
            }
            acceptedRequestId = enrollmentRequest.Id;
            Course activeCourse = courseService.Get(enrollmentRequest.CourseId);
            Course = new(activeCourse);
            return true;
        }

        public void WithdrawalFromCourse()
        {
            var withdrawalService = new WithdrawalRequestService();
            if (withdrawalService.AlreadyExists(acceptedRequestId))
            {
                MessageBox.Show("Request already submitted. Wait for response.");
                return;
            }
            WithdrawalRequestWindow wrWindow = new(acceptedRequestId);
            wrWindow.Show();
        }

        public bool DisableCourseWithdrawal()
        {
            var enrollmentService = new EnrollmentRequestService();
            var withdrawalService = new WithdrawalRequestService();

            return !enrollmentService.CanRequestWithdrawal(acceptedRequestId) || withdrawalService.AlreadyExists(acceptedRequestId);
        }
    }
}
