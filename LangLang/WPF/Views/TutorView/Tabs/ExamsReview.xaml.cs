using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.View;
using LangLang.View.CourseGUI;
using LangLang.View.ExamSlotGUI;
using LangLang.View.StudentGUI.Tabs;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.ExamViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LangLang.WPF.Views.TutorView.Tabs
{
    /// <summary>
    /// Interaction logic for Exams.xaml
    /// </summary>
    public partial class ExamsReview : UserControl
    {
        public ExamSlotsTutorViewModel ExamsTutorVM { get; set; }
        public ExamsReview(Tutor LoggedIn)
        {
            InitializeComponent();
            ExamsTutorVM = new ExamSlotsTutorViewModel(LoggedIn);
            DataContext = ExamsTutorVM;

            Update();
        }
        public void Update()
        {
            ExamsTutorVM.SetDataForReview();
        }
        private void ExamSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotCreateWindow createWindow = new(ExamsTutorVM.LoggedIn,this);
            createWindow.Show();
        }
        private void ExamSlotUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamsTutorVM.UpdateExam(this);
        }
        private void ExamSlotDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamsTutorVM.DeleteExam();
        }
        private void DisableButtonsES()
        {
            deleteExamBtn.IsEnabled = false;
            updateExamBtn.IsEnabled = false;
            examApplicationBtn.IsEnabled = false;
            enterResultsBtn.IsEnabled = false;
        }

        private void EnableButtonsES()
        {
            deleteExamBtn.IsEnabled = true;
            updateExamBtn.IsEnabled = true;
            examApplicationBtn.IsEnabled = true;
            enterResultsBtn.IsEnabled = true;
        }
        private void ExamSlotsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ExamsTutorVM.IsExamSelected()) // when the DataGrid listener is triggered, check if there is a selection, and based on that, decide whether to enable or disable the buttons
            {
                DisableButtonsES();
            }
            else
            {
                EnableButtonsES();
            }
        }
        private void EnterGradeBtn_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (courseController.GetEnd(SelectedCourse.ToCourse()) < DateTime.Now)
            {
                EnterGradesWindow gradesWindow = new(appController, SelectedCourse);
                gradesWindow.Show();
            }
            else
            {
                MessageBox.Show("The course is not finished.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            */
        }
        private void DurationOfCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (courseController.IsActive(SelectedCourse.ToCourse()))
            {
                DurationOfCourseWindow courseWindow = new(appController, SelectedCourse);
                courseWindow.Show();
            }
            else
            {
                MessageBox.Show("The course is not active.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            */
        }
        private void ExamSlotSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            //ExamSlotSearchWindow searchWindow = new( LoggedIn);
            //searchWindow.Show();
        }

        

        private void ButtonSeeApplications_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (ExamSlotService.ApplicationsVisible(SelectedExamSlot.Id) && SelectedExamSlot.Applicants != 0)
            {
                ExamApplications applicationsWindow = new(appController, SelectedExamSlot);
                applicationsWindow.Show();
            }
            else
            {
                MessageBox.Show($"If there are applications, they can only be viewed {Constants.PRE_START_VIEW_PERIOD} days before exam and during the exam.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            */
        }

        private void ButtonEnterResults_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (SelectedExamSlot.ExamDate.AddHours(Constants.EXAM_DURATION) < DateTime.Now) // after the EXAM_DURATION-hour exam concludes, it is possible to open a window.
            {
                EnterResults resultsWindow = new(appController, SelectedExamSlot);
                resultsWindow.Show();
            }
            else
            {
                MessageBox.Show("This window can be opened once the exam is passed!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            */
        }

    }
}
