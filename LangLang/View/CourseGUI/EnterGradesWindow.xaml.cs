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
using LangLang.Core.Model.DAO;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for EnterGradesWindow.xaml
    /// </summary>
    public partial class EnterGradesWindow : Window
    {
        public StudentViewModel SelectedStudent { get; set; }
        public ObservableCollection<StudentViewModel> Students { get; set; }

        private CourseViewModel course;
        public EnterGradesWindow( CourseViewModel course)
        {
            InitializeComponent();
            DataContext = this;

            //this.appController = appController;
            this.course = course;

            Students = new();

            DisableForm();
            Update();
        }
        public void Update()
        {
            Students.Clear();
            EnrollmentRequestService enrollmentRequestService = new();
            WithdrawalRequestService withdrawalRequestService = new();
            GradeService gradeService = new();
            foreach (EnrollmentRequest enrollment in enrollmentRequestService.GetByCourse(course.ToCourse()))
            {
                // All studnets that attend the course (do not have accepted withdrawals)
                // and have not been graded
                if (enrollment.Status == Status.Accepted && !withdrawalRequestService.HasAcceptedWithdrawal(enrollment.Id))
                {
                    bool graded = false;
                    foreach (Grade grade in gradeService.GetByCourse(course.ToCourse()))
                    {
                        if (enrollment.StudentId == grade.StudentId)
                        {
                            graded = true;
                            break;
                        }
                    }
                    if (!graded)
                    {
                        var studentService = new StudentService();
                        Students.Add(new StudentViewModel(studentService.Get(enrollment.StudentId)));
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
                    //gradeContoller.Add(new Grade(0, course.Id, SelectedStudent.Id, (int)selectedNumber)); //TODO: Implement addition through DTO class when it's implemented
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
