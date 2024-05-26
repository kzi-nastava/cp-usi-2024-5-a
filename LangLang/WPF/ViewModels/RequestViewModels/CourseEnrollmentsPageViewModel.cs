using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.RequestsViewModels;
using LangLang.WPF.Views.StudentView;
using LangLang.WPF.Views.TutorView.AddidtionalWindows.CourseView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.WPF.ViewModels.RequestViewModels
{
    public class CourseEnrollmentsPageViewModel
    {
        public EnrollmentRequestViewModel SelectedEnrollmentRequest { get; set; }
        private CourseViewModel Course { get; set; }
        public ObservableCollection<EnrollmentRequestViewModel> CourseRequests { get; set; }
        public List<EnrollmentRequest> RequestsForReview { get; set; }
        public CourseEnrollmentsPageViewModel(CourseViewModel course)
        {
            Course = course;
            CourseRequests = new();
        }
        public void Update()
        {
            CourseRequests.Clear();
            EnrollmentRequestService enrollmentRequestService = new();
            RequestsForReview = enrollmentRequestService.GetByCourse(Course.ToCourse());
            foreach (EnrollmentRequest enrollment in RequestsForReview)
            {
                if (enrollment.Status == Status.Pending)
                {
                    CourseRequests.Add(new EnrollmentRequestViewModel(enrollment));
                }
            }
        }

        public void AcceptEnrollments()
        {
            // if the course is not online and the number of enrollments excedes the maximal number of students
            if (Course.NotOnline && RequestsForReview.Count > Course.ToCourse().MaxStudents)
            {
                MessageBox.Show("You have exceded the maximal number of students for this course.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            foreach (EnrollmentRequest enrollment in RequestsForReview)
            {
                if (enrollment.Status != Status.Rejected)
                {
                    enrollment.UpdateStatus(Status.Accepted);
                    EnrollmentRequestService enrollmentRequestService = new();
                    enrollmentRequestService.Update(enrollment);
                    NotifyStudentAboutAcceptence(enrollment.StudentId);
                }
            }
            UpdateCourse(false, RequestsForReview.Count);
        }
        private void UpdateCourse(bool modifiable, int studentsCount)
        {
            Course.Modifiable = modifiable;
            Course.NumberOfStudents = studentsCount;
            CourseService courseService = new();
            courseService.Update(Course.ToCourse());
        }

        public void RejectEnrollment()
        {
            EnrollmentRequest enrollment = SelectedEnrollmentRequest.ToEnrollmentRequest();
            enrollment.UpdateStatus(Status.Rejected);
            EnrollmentRequestService enrollmentRequestService = new();
            enrollmentRequestService.Update(enrollment);
            Update();
            InputPopupWindow inputPopup = new InputPopupWindow();
            inputPopup.ShowDialog();
            NotifyStudentAboutRejection(enrollment.StudentId, inputPopup.EnteredText);
        }
        private void NotifyStudentAboutAcceptence(int studentId)
        {
            var studentService = new StudentService();
            var student = studentService.Get(studentId);
            var mailMessage = $"You have been accepted to the course: {Course.Language} {Course.Level}";

            try
            {
                EmailService.SendEmail(student.Profile.Email, $"Accepted: {Course.Language} {Course.Level}", mailMessage);
            }
            catch (SmtpException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void NotifyStudentAboutRejection(int studentId, string reason)
        {
            var studentService = new StudentService();
            var student = studentService.Get(studentId);
            var mailMessage = $"You have been rejected from the course: {Course.Language} {Course.Level}. Reason: {reason}";

            try
            {
                EmailService.SendEmail(student.Profile.Email, $"Request rejected: {Course.Language} {Course.Level}", mailMessage);
            }
            catch (SmtpException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
