using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LangLang.View.StudentGUI.Tabs
{
    public partial class AvailableExams : UserControl
    {
        private readonly StudentWindow parentWindow;
        private readonly AppController appController;
        private readonly ExamSlotController examController;
        private readonly Student currentlyLoggedIn;
        public List<ExamSlot> ExamsForReview { get; set; }
        public ObservableCollection<ExamSlotDTO> ExamSlots { get; set; }
        private ExamAppRequest Request { get; set; }
        public ExamSlotDTO SelectedExam { get; set; }

        public AvailableExams(AppController appController, Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            DataContext = this;
            this.parentWindow = parentWindow;
            this.appController = appController;
            examController = appController.ExamSlotController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            SetDataForReview();
            ExamSlots = new();
            levelExamcb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        public void SetDataForReview()
        {
            //TODO: Update to show only available exams tat student hasn't applied for
            ExamsForReview = examController.GetAvailableExams(currentlyLoggedIn, appController);
        }
        private void SearchExams(object sender, RoutedEventArgs e)
        {
            string? language = languageExamtb.Text;
            LanguageLevel? level = null;
            if (levelExamcb.SelectedValue != null)
                level = (LanguageLevel)levelExamcb.SelectedValue;
            DateTime examDate = examdatePicker.SelectedDate ?? default;

            ExamsForReview = examController.SearchExamsByStudent(appController, currentlyLoggedIn, examDate, language, level); ;
            parentWindow.Update();
        }

        private void ClearExamBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamsForReview = examController.GetAvailableExams(currentlyLoggedIn, appController);
            levelExamcb.SelectedItem = null;
            parentWindow.Update();
        }
        private void SendRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            bool canApplyForExams = appController.StudentController.CanApplyForExams(currentlyLoggedIn, appController);
            if (canApplyForExams)
            {
                if (SelectedExam == null) return;
                Request = new();
                Request.ExamSlotId = SelectedExam.ToExamSlot().Id;
                Request.StudentId = currentlyLoggedIn.Id;
                Request.SentAt = DateTime.Now;
                var examAppRequestController = appController.ExamAppRequestController;
                examAppRequestController.Add(Request, examController);

                MessageBox.Show("You successfully applied for exam.");

                SetDataForReview();
                parentWindow.examApplicationsTab.SetDataForReview();
                parentWindow.Update();
            }
            else
            {
                MessageBox.Show("Can't apply for the exam as all results have not yet been received.");
            }
            
        }
    }
}
