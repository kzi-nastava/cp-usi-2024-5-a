using LangLang.Core;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.Views.StudentView;
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
        public ExamApplicationViewModel SelectedApplication { get; set; }
        public ObservableCollection<ExamApplicationViewModel> Applications { get; set; }
        public List<ExamApplication> ApplicationsForReview { get; set; }

        private ExamApplicationController applicationController {get; set;}
        private ExamSlotController examSlotController;
        private Student currentlyLoggedIn;

        public ExamApplications(Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            DataContext = this;

            this.parentWindow = parentWindow;
            this.currentlyLoggedIn = currentlyLoggedIn;
            applicationController = new ExamApplicationController();
            examSlotController = new ExamSlotController();

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
                Applications.Add(new ExamApplicationViewModel(application));
            }
        }

        private void CancelApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to cancel exam application?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                bool canceled = applicationController.CancelApplication(SelectedApplication.ToExamApplication(), examSlotController);
                if (canceled)
                {
                    MessageBox.Show("Cancelation was successful.");
                    SetDataForReview();
                    //parentWindow.availableExamsTab.SetDataForReview();
                    parentWindow.Update();
                }
                else
                    MessageBox.Show($"Can't cancel exam. There is less than {Constants.EXAM_CANCELATION_PERIOD} days or exam has passed");

            }
        }
    }
}
