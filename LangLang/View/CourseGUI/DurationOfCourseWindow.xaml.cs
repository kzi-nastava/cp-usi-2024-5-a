using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using LangLang.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Automation;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Converters;
using LangLang.BusinessLogic.UseCases;
using LangLang.WPF.ViewModels.StudentViewModels;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.RequestsViewModels;

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

        private AppController appController;
        private CourseService courseService;
        private WithdrawalRequestService withdrawalReqService;
        private EnrollmentRequestService enrollmentReqService;
        private PenaltyPointController penaltyPointController;
        private TutorService tutorService;
        private CourseViewModel course;
        public DurationOfCourseWindow(AppController appController, CourseViewModel course)
        {
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            this.course = course;
            withdrawalReqService = new();
            courseService = new();
            penaltyPointController = appController.PenaltyPointController;
            tutorService = new();
            enrollmentReqService = new();

            Students = new();
            Withdrawals = new();

            DisableStudentsForm();
            DisableWithdrawalForm();
            Update();
        }
        public void Update()
        {
            Students.Clear();
            //All studnets that attend the course and do not have accepted withdrawals
            foreach (EnrollmentRequest enrollment in enrollmentReqService.GetByCourse(course.ToCourse()))
            {
                if (enrollment.Status == Status.Accepted && !withdrawalReqService.HasAcceptedWithdrawal(enrollment.Id))
                {
                    var studentService = new StudentService();
                    Students.Add(new StudentViewModel(studentService.Get(enrollment.StudentId)));
                }
            }
            Withdrawals.Clear();
            foreach (WithdrawalRequest withdrawal in withdrawalReqService.GetByCourse(course.ToCourse()))
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
            // Implement once the email sending functionality is added.
        }

        private void PenaltyBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to give the student a penalty point?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (penaltyPointController.HasGivenPenaltyPoint(SelectedStudent.ToStudent(), tutorService.Get(course.TutorId), course.ToCourse(), appController))
                    MessageBox.Show("You have already given the student a penalty point today.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    penaltyPointController.GivePenaltyPoint(SelectedStudent.ToStudent(), tutorService.Get(course.TutorId), course.ToCourse(), appController);
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
                courseService.RemoveStudent(course.Id);
                withdrawalReqService.UpdateStatus(SelectedWithdrawal.Id, Status.Accepted);
                ShowSuccess();
                Update();
            }
        }
        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to reject the withdrawal?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                withdrawalReqService.UpdateStatus(SelectedWithdrawal.Id, Status.Rejected);
                courseService.RemoveStudent(course.Id);
                penaltyPointController.GivePenaltyPoint(SelectedStudent.ToStudent(), tutorService.Get(course.TutorId), course.ToCourse(), appController);
                NotifyStudentAboutPenaltyPoint(SelectedStudent.Id);

                ShowSuccess();
                Update();
            }
        }
    }
}
