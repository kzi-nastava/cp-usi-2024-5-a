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
    public partial class ExamApplications : UserControl
    {
        private readonly StudentWindow parentWindow;
        public ExamApplicationDTO SelectedApplication { get; set; }
        public ObservableCollection<ExamApplicationDTO> Applications { get; set; }
        public List<ExamApplication> ApplicationsForReview { get; set; }

        private AppController appController;
        private ExamApplicationController applicationController;
        private ExamSlotController examSlotController;
        private StudentController studentController;
        private Student currentlyLoggedIn;

        public ExamApplications(AppController appController, Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            DataContext = this;

            this.parentWindow = parentWindow;
            this.appController = appController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            applicationController = appController.ExamApplicationController;
            examSlotController = appController.ExamSlotController;
            studentController = appController.StudentController;

            Applications = new();

            SetDataForReview();
        }

        public void SetDataForReview()
        {
            ApplicationsForReview = applicationController.GetApplications(currentlyLoggedIn);
        }
        public void Update()
        {
            Applications.Clear();
            foreach (ExamApplication application in applicationController.GetApplications(currentlyLoggedIn))
            {
                Applications.Add(new ExamApplicationDTO(application, appController));
            }
        }   

        private void CancelApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to cancel exam application?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                appController.ExamApplicationController.CancelApplication(SelectedApplication.ToExamApplication(), examSlotController);
                MessageBox.Show("Cancelation was successful.");
                SetDataForReview();
                parentWindow.availableExamsTab.SetDataForReview();
                parentWindow.Update();
            }
        }
    }
}
