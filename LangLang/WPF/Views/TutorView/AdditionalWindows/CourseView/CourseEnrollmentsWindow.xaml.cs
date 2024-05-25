using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.RequestsViewModels;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for CourseEnrollmentsWindow.xaml
    /// </summary>
    public partial class CourseEnrollmentsWindow : Window
    {
        public EnrollmentRequestViewModel SelectedEnrollment { get; set; }
        public ObservableCollection<EnrollmentRequestViewModel> Enrollments { get; set; }

        private CourseViewModel course;
        public CourseEnrollmentsWindow(CourseViewModel course)
        {
            InitializeComponent();
            DataContext = this;
            this.course = course;
            Enrollments = new();

            rejectBtn.IsEnabled = false;
            conifrmListBtn.IsEnabled = true;

            Update();
        }

        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableForm();
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to reject " + SelectedEnrollment.StudentName + " " + SelectedEnrollment.StudentLastName + " from the course?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                EnrollmentRequest enrollment = SelectedEnrollment.ToEnrollmentRequest();
                enrollment.UpdateStatus(Status.Rejected);
                EnrollmentRequestService enrollmentRequestService = new();
                enrollmentRequestService.Update(enrollment);
                Update();
                InputPopupWindow inputPopup = new InputPopupWindow();
                inputPopup.ShowDialog();
                NotifyStudentAboutRejection(enrollment.StudentId, inputPopup.EnteredText);
                ShowSuccess();
            }
            conifrmListBtn.IsEnabled = true;
        }

        public void Update()
        {
            Enrollments.Clear();
            EnrollmentRequestService enrollmentRequestService = new();
            foreach (EnrollmentRequest enrollment in enrollmentRequestService.GetByCourse(course.ToCourse()))
            {
                if(enrollment.Status == Status.Pending)
                {
                    Enrollments.Add(new EnrollmentRequestViewModel(enrollment));
                }
            }
        }

        private void AcceptListBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to confirm list?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // if the course is not online and the number of enrollments excedes the maximal number of students
                if(course.NotOnline && Enrollments.Count > course.ToCourse().MaxStudents)
                    MessageBox.Show("You have exceded the maximal number of students for this course.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                { 
                    foreach (EnrollmentRequestViewModel enrollmentDTO in Enrollments)
                    {
                        EnrollmentRequest enrollment = enrollmentDTO.ToEnrollmentRequest();
                        if(enrollment.Status != Status.Rejected)
                        {
                            enrollment.UpdateStatus(Status.Accepted);
                            EnrollmentRequestService enrollmentRequestService = new();
                            enrollmentRequestService.Update(enrollment);
                            NotifyStudentAboutAcceptence(enrollment.StudentId);
                        }
                    }
                    UpdateCourse(false, Enrollments.Count);
                    DisableForm();
                    ShowSuccess();
                    Close();
                }
            }
        }
        private void UpdateCourse(bool modifiable, int studentsCount)
        {
            course.Modifiable = modifiable;
            course.NumberOfStudents = studentsCount;
            CourseService courseService = new();
            courseService.Update(course.ToCourse());
        }
        private void NotifyStudentAboutAcceptence(int studentId)
        {
            var studentService = new StudentService();
            var student = studentService.Get(studentId);
            var mailMessage = $"You have been accepted to the course: {course.Language} {course.Level}";

            try
            {
                EmailService.SendEmail(student.Profile.Email, $"Accepted: {course.Language} {course.Level}", mailMessage);
            } catch (SmtpException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void NotifyStudentAboutRejection(int studentId, string reason)
        {
            var studentService = new StudentService();
            var student = studentService.Get(studentId);
            var mailMessage = $"You have been rejected from the course: {course.Language} {course.Level}. Reason: {reason}";

            try
            {
                EmailService.SendEmail(student.Profile.Email, $"Request rejected: {course.Language} {course.Level}", mailMessage);
            }
            catch (SmtpException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void EnrollmentTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedEnrollment != null && course.Modifiable)
                rejectBtn.IsEnabled = true;
            else
                rejectBtn.IsEnabled = false;
        }
        private void DisableForm()
        {
            rejectBtn.IsEnabled = false;
            conifrmListBtn.IsEnabled = false;
        }
    }
}
