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
        public ExamAppRequestDTO SelectedApplication { get; set; }
        public ObservableCollection<ExamAppRequestDTO> Applications { get; set; }
        public List<ExamAppRequest> ApplicationsForReview { get; set; }

        private AppController appController;
        private ExamAppRequestController applicationController;
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
            applicationController = appController.ExamAppRequestController;
            examSlotController = appController.ExamSlotController;
            studentController = appController.StudentController;

            Applications = new();

            SetDataForReview();
        }

        public void SetDataForReview()
        {
            ApplicationsForReview = applicationController.GetStudentRequests(currentlyLoggedIn.Profile.Id);
        }
        public void Update()
        {
            Applications.Clear();
            foreach (ExamAppRequest application in applicationController.GetStudentRequests(currentlyLoggedIn.Profile.Id))
            {
                Applications.Add(new ExamAppRequestDTO(application, appController));
            }
        }   

        private void CancelRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to cancel exam application?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                appController.ExamAppRequestController.CancelRequest(SelectedApplication.ToExamAppRequest(), examSlotController);
                MessageBox.Show("Cancelation was successful.");
                SetDataForReview();
                parentWindow.availableExamsTab.SetDataForReview();
                parentWindow.Update();
            }
        }
    }
}
