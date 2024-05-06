using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
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
using static System.Net.Mime.MediaTypeNames;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for CourseEnrollmentsWindow.xaml
    /// </summary>
    public partial class CourseEnrollmentsWindow : Window
    {
        public EnrollmentRequestDTO SelectedEnrollment { get; set; }
        public ObservableCollection<EnrollmentRequestDTO> Enrollments { get; set; }
        private AppController appController;
        private EnrollmentRequestController enrollmentController;
        private CourseController courseController;
        private StudentController studentController;
        private TutorController tutorController;
        private MessageController messageController;
        private CourseDTO course;
        public CourseEnrollmentsWindow(AppController appController, CourseDTO course)
        {
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            this.course = course;
            enrollmentController = appController.EnrollmentRequestController;
            courseController = appController.CourseController;
            studentController = appController.StudentController;
            tutorController = appController.TutorController;
            messageController = appController.MessageController;

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
                enrollmentController.Update(enrollment);
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
            foreach (EnrollmentRequest enrollment in enrollmentController.GetEnrollments(course.Id))
            {
                if(enrollment.Status == Status.Pending)
                {
                    Enrollments.Add(new EnrollmentRequestDTO(enrollment, appController));
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
                {
                    MessageBox.Show("You have exceded the maximal number of students for this course.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                { 
                    foreach (EnrollmentRequestDTO enrollmentDTO in Enrollments)
                    {
                        EnrollmentRequest enrollment = enrollmentDTO.ToEnrollmentRequest();
                        if(enrollment.Status != Core.Model.Enums.Status.Rejected)
                        {
                            enrollment.UpdateStatus(Status.Accepted);
                            enrollmentController.Update(enrollment);
                            NotifyStudentAboutAcceptence(enrollment.StudentId);
                        }
                    }
                    course.Modifiable = false;
                    course.NumberOfStudents = Enrollments.Count;
                    courseController.Update(course.ToCourse());
                    rejectBtn.IsEnabled = false;
                    conifrmListBtn.IsEnabled = false;
                    ShowSuccess();
                    Close();
                }
            }
        }
        private void NotifyStudentAboutAcceptence(int studentId)
        {
            Message message = new Message(0, tutorController.GetById(course.TutorId).Profile, studentController.GetById(studentId).Profile, "You have been accepted to the course: " + course.Id + " " + course.Language);
            messageController.Add(message);
        }
        private void NotifyStudentAboutRejection(int studentId, string reason)
        {
            Message message = new Message(0, tutorController.GetById(course.TutorId).Profile, studentController.GetById(studentId).Profile, "You have been rejected from the course: Id " + course.Id + ", " + course.Language + ". The reason: "+reason);
            messageController.Add(message);
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
