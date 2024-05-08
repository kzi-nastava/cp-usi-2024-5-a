using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using LangLang.Core.Model;
using LangLang.DTO;
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

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for DurationOfCourseWindow.xaml
    /// </summary>
    public partial class DurationOfCourseWindow : Window
    {
        public StudentDTO SelectedStudent { get; set; }
        public WithdrawalRequestDTO SelectedWithdrawal { get; set; }
        public ObservableCollection<StudentDTO> Students { get; set; }
        public ObservableCollection<WithdrawalRequestDTO> Withdrawals { get; set; }

        private AppController appController;
        private CourseController courseController;
        private StudentController studentController;
        private WithdrawalRequestController withdrawalController;
        private EnrollmentRequestController enrollmentController;
        private PenaltyPointController penaltyPointController;
        private TutorController tutorController;
        private MessageController messageController;
        private CourseDTO course;
        public DurationOfCourseWindow(AppController appController, CourseDTO course)
        {
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            this.course = course;
            withdrawalController = appController.WithdrawalRequestController;
            courseController = appController.CourseController;
            studentController = appController.StudentController;
            penaltyPointController = appController.PenaltyPointController;
            tutorController = appController.TutorController;
            enrollmentController = appController.EnrollmentRequestController;
            messageController = appController.MessageController;

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
            foreach (EnrollmentRequest enrollment in enrollmentController.GetRequests(course.ToCourse()))
            {
                if (enrollment.Status == Status.Accepted && !withdrawalController.HasAcceptedWithdrawal(enrollment.Id))
                {
                    Students.Add(new StudentDTO(studentController.Get(enrollment.StudentId)));
                }
            }
            Withdrawals.Clear();
            foreach (WithdrawalRequest withdrawal in withdrawalController.GetRequests(course.ToCourse(), enrollmentController))
            {
                if(withdrawal.Status == Status.Pending)
                {
                    Withdrawals.Add(new WithdrawalRequestDTO(withdrawal, appController));
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
            Message message = new Message(0, tutorController.Get(course.TutorId).Profile, studentController.Get(studentId).Profile, "You have recieved one penalty point in course: " + course.Id + " " + course.Language);
            messageController.Add(message);
        }

        private void PenaltyBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to give the student a penalty point?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (penaltyPointController.HasGivenPenaltyPoint(SelectedStudent.ToStudent(), tutorController.Get(course.TutorId), course.ToCourse(), appController))
                    MessageBox.Show("You have already given the student a penalty point today.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    penaltyPointController.GivePenaltyPoint(SelectedStudent.ToStudent(), tutorController.Get(course.TutorId), course.ToCourse(), appController);
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
                courseController.RemoveStudent(course.Id);
                withdrawalController.UpdateStatus(SelectedWithdrawal.Id, Status.Accepted);
                ShowSuccess();
                Update();
            }
        }
        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to reject the withdrawal?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                withdrawalController.UpdateStatus(SelectedWithdrawal.Id, Status.Rejected);
                courseController.RemoveStudent(course.Id);
                penaltyPointController.GivePenaltyPoint(SelectedStudent.ToStudent(), tutorController.Get(course.TutorId), course.ToCourse(), appController);
                NotifyStudentAboutPenaltyPoint(SelectedStudent.Id);

                ShowSuccess();
                Update();
            }
        }
    }
}
