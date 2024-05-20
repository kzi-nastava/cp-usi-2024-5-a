using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.DTO;
using LangLang.WPF.Views.StudentView.AdditionalWindows;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModel
{
    public class ActiveCourseViewModel
    {
        public CourseDTO Course { get; set; } // TODO: rename CourseDTO -> CourseViewModel
        private Student currentlyLoggedIn;
        private int acceptedRequestId;
        public ActiveCourseViewModel(Student currentlyLoggedIn)
        {
            this.currentlyLoggedIn = currentlyLoggedIn;
        }

        public bool SetCourse()
        {
            var enrollmentService = new EnrollmentRequestService();
            var courseService = new CourseController();

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
