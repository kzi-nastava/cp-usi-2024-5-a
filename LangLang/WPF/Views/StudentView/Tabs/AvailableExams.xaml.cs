using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.Views;
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
        public ObservableCollection<ExamSlotViewModel> ExamSlots { get; set; }
        private ExamApplication Application { get; set; }
        public ExamSlotViewModel SelectedExam { get; set; }

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

            ExamsForReview = examController.SearchByStudent(appController, currentlyLoggedIn, examDate, language, level); ;
            parentWindow.Update();
        }

        private void ClearExamBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamsForReview = examController.GetAvailableExams(currentlyLoggedIn, appController);
            levelExamcb.SelectedItem = null;
            parentWindow.Update();
        }
        private void SendApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            var studentService = new StudentService();
            bool canApplyForExams = studentService.CanApplyForExams(currentlyLoggedIn);
            if (canApplyForExams)
            {
                if (SelectedExam == null) return;
                Application = new();
                Application.ExamSlotId = SelectedExam.ToExamSlot().Id;
                Application.StudentId = currentlyLoggedIn.Id;
                Application.SentAt = DateTime.Now;
                var examApplicationController = appController.ExamApplicationController;
                examApplicationController.Add(Application, examController);

                MessageBox.Show("You successfully applied for exam.");

                SetDataForReview();
                //parentWindow.examApplicationsTab.SetDataForReview();
                parentWindow.Update();
            }
            else
            {
                MessageBox.Show("Can't apply for the exam as all results have not yet been received.");
            }

        }
    }
}
