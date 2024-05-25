using LangLang.Domain.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LangLang.BusinessLogic.UseCases;
using LangLang.WPF.ViewModels.StudentViewModels;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.RequestsViewModels;
using LangLang.Core.Model.DAO;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for DurationOfCourseWindow.xaml
    /// </summary>
    public partial class DurationOfCourseWindow : Window
    {
        public StudentViewModel SelectedStudent { get; set; }
        public WithdrawalRequestViewModel SelectedWithdrawal { get; set; }
        public ObservableCollection<StudentViewModel> Students { get; set; }
        public ObservableCollection<WithdrawalRequestViewModel> Withdrawals { get; set; }

        private CourseViewModel course;
        public DurationOfCourseWindow(CourseViewModel course)
        {
            InitializeComponent();
            DataContext = this;

            this.course = course;

            Students = new();
            Withdrawals = new();

            DisableStudentsForm();
            DisableWithdrawalForm();
            Update();
        }
        public void Update()
        {
            Students.Clear();

            EnrollmentRequestService enrollmentRequestService = new();
            WithdrawalRequestService withdrawalRequestService = new();
            //All studnets that attend the course and do not have accepted withdrawals
            foreach (EnrollmentRequest enrollment in enrollmentRequestService.GetByCourse(course.ToCourse()))
            {
                if (enrollment.Status == Status.Accepted && !withdrawalRequestService.HasAcceptedWithdrawal(enrollment.Id))
                {
                    var studentService = new StudentService();
                    Students.Add(new StudentViewModel(studentService.Get(enrollment.StudentId)));
                }
            }
            Withdrawals.Clear();
            foreach (WithdrawalRequest withdrawal in withdrawalRequestService.GetByCourse(course.ToCourse()))
            {
                if(withdrawal.Status == Status.Pending)
                {
                    Withdrawals.Add(new WithdrawalRequestViewModel(withdrawal));
                }
            }
        }
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnableStudnetsForm()
        {
            penaltyPointBtn.IsEnabled = true;
        }
        private void DisableStudentsForm()
        {
            penaltyPointBtn.IsEnabled = false;
        }
        private void EnableWithdrawalForm()
        {
            acceptBtn.IsEnabled = true;
            rejectBtn.IsEnabled = true;
        }
        private void DisableWithdrawalForm()
        {
            acceptBtn.IsEnabled = false;
            rejectBtn.IsEnabled = false;
        }
        private void StudentTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedStudent != null)
                EnableStudnetsForm();
            else
                DisableStudentsForm();
        }
        private void WithdrawalTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedWithdrawal != null)
                EnableWithdrawalForm();
            else
                DisableWithdrawalForm();
        }
        private void NotifyStudentAboutPenaltyPoint(int studentId)
        {
            var studentService = new StudentService();
            var student = studentService.Get(studentId); 
            var mailMessage = $"You have received one penalty point in course: {course.Language}, level: {course.Level}";

            EmailService.SendEmail(student.Profile.Email, "Penalty point", mailMessage);
        }

        private void PenaltyBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to give the student a penalty point?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                PenaltyPointService penaltyPointService = new();
                TutorService tutorService = new();
                if (penaltyPointService.HasGivenPenaltyPoint(SelectedStudent.ToStudent(), tutorService.Get(course.TutorId), course.ToCourse()))
                    MessageBox.Show("You have already given the student a penalty point today.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    penaltyPointService.GivePenaltyPoint(SelectedStudent.ToStudent(), tutorService.Get(course.TutorId), course.ToCourse());
                    NotifyStudentAboutPenaltyPoint(SelectedStudent.Id);
                    
                    ShowSuccess();
                }
            }
        }
        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to accept the withdrawal?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                WithdrawalRequestService withdrawalRequestService = new();
                CourseService courseService = new CourseService();
                courseService.RemoveStudent(course.Id);
                withdrawalRequestService.UpdateStatus(SelectedWithdrawal.Id, Status.Accepted);
                ShowSuccess();
                Update();
            }
        }
        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to reject the withdrawal?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                WithdrawalRequestService withdrawalRequestService = new();
                PenaltyPointService penaltyPointService = new();
                withdrawalRequestService.UpdateStatus(SelectedWithdrawal.Id, Status.Rejected);

                CourseService courseService = new();
                TutorService tutorService = new();
                courseService.RemoveStudent(course.Id);
                penaltyPointService.GivePenaltyPoint(SelectedStudent.ToStudent(), tutorService.Get(course.TutorId), course.ToCourse());

                NotifyStudentAboutPenaltyPoint(SelectedStudent.Id);

                ShowSuccess();
                Update();
            }
        }
    }
}
