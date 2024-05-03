using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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

        public AvailableExams(AppController appController, Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            this.parentWindow = parentWindow;
            this.appController = appController;
            examController = appController.ExamSlotController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            InitializeComponent();
            SetDataForReview();
            ExamSlots = new();
            levelExamcb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        private void SetDataForReview()
        {
            var studentController = appController.StudentController;
            var enrollmentController = appController.EnrollmentRequestController;
            var courseController = appController.CourseController;
            ExamsForReview = studentController.GetAvailableExams(currentlyLoggedIn, courseController, examController, enrollmentController);
        }
        private void SearchExams(object sender, RoutedEventArgs e)
        {
            string? language = languageExamtb.Text;
            LanguageLevel? level = null;
            if (levelExamcb.SelectedValue != null)
                level = (LanguageLevel)levelExamcb.SelectedValue;
            DateTime examDate = examdatePicker.SelectedDate ?? default;

            var studentController = appController.StudentController;
            var enrollmentController = appController.EnrollmentRequestController;
            var courseController = appController.CourseController;

            ExamsForReview = studentController.SearchExamSlotsByStudent(examController, courseController, enrollmentController, currentlyLoggedIn.Id, examDate, language, level); ;
            parentWindow.Update();
        }

        private void ClearExamBtn_Click(object sender, RoutedEventArgs e)
        {
            var courseController = appController.CourseController;
            var studentController = appController.StudentController;
            var enrollmentController = appController.EnrollmentRequestController;
            ExamsForReview = studentController.GetAvailableExams(currentlyLoggedIn, courseController, examController, enrollmentController);
            levelExamcb.SelectedItem = null;
            parentWindow.Update();
        }
    }
}
