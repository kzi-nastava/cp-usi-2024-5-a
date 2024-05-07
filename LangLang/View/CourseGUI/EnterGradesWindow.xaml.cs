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
    /// Interaction logic for EnterGradesWindow.xaml
    /// </summary>
    public partial class EnterGradesWindow : Window
    {
        public StudentDTO SelectedStudent { get; set; }
        public ObservableCollection<StudentDTO> Students { get; set; }

        private AppController appController;
        private CourseController courseController;
        private StudentController studentController;
        private WithdrawalRequestController withdrawalController;
        private EnrollmentRequestController enrollmentController;
        private PenaltyPointController penaltyPointController;
        private TutorController tutorController;
        private MessageController messageController;
        private GradeController gradeContoller;
        private CourseDTO course;
        public EnterGradesWindow(AppController appController, CourseDTO course)
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
            gradeContoller = appController.GradeController;

            Students = new();

            DisableForm();
            Update();
        }
        public void Update()
        {
            Students.Clear();
            foreach (EnrollmentRequest enrollment in enrollmentController.GetRequests(course.ToCourse()))
            {
                // All studnets that attend the course (do not have accepted withdrawals)
                // and have not been graded
                if (enrollment.Status == Status.Accepted && !withdrawalController.HasAcceptedWithdrawal(enrollment.Id))
                {
                    bool graded = false;
                    foreach (Grade grade in gradeContoller.GetGradesWithCourse(course.Id))
                    {
                        if (enrollment.StudentId == grade.StudentId)
                        {
                            graded = true;
                            break;
                        }
                    }
                    if (!graded)
                    {
                        Students.Add(new StudentDTO(studentController.Get(enrollment.StudentId)));
                    }
                }
            }
        }
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnableForm()
        {
            gradeBtn.IsEnabled = true;
        }
        private void DisableForm()
        {
            gradeBtn.IsEnabled = false;
        }
        private void StudentTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedStudent != null)
                EnableForm();
            else
                DisableForm();
        }
        private void GradeBtn_Click(object sender, RoutedEventArgs e)
        {
            GradeSelectionPopup popup = new GradeSelectionPopup();

            bool? result = popup.ShowDialog();
            if (result == true)
            {
                int? selectedNumber = popup.SelectedNumber;

                if (selectedNumber.HasValue)
                {
                    gradeContoller.Add(new Grade(0, course.Id, SelectedStudent.Id, (int)selectedNumber));
                    ShowSuccess();
                }
                else
                {
                    MessageBox.Show("No number selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            Update();
        }
    }
}
