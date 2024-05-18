using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.DTO;
using LangLang.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI.Tabs
{
    public partial class EnrollmentRequests : UserControl
    {
        public EnrollmentRequestDTO SelectedEnrollmentRequest { get; set; }
        private AppController appController { get; set; }
        private Student currentlyLoggedIn { get; set; }
        private StudentWindow parentWindow { get; set; }
        public ObservableCollection<EnrollmentRequestDTO> StudentRequests { get; set; }
        public List<EnrollmentRequest> RequestsForReview { get; set; }
        public EnrollmentRequests(AppController appController, Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            DataContext = this;
            this.appController = appController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            this.parentWindow = parentWindow;
            SetDataForReview();
            StudentRequests = new();
            CancelRequestBtn.IsEnabled = false;
        }


        public void SetDataForReview()
        {
            var enrollmentController = appController.EnrollmentRequestController;
            RequestsForReview = enrollmentController.GetRequests(currentlyLoggedIn);
        }

        private void CancelRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the request?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TryCancelRequest();
            }
        }

        private void TryCancelRequest()
        {
            EnrollmentRequest enrollmentRequest = SelectedEnrollmentRequest.ToEnrollmentRequest();

            var enrollmentController = appController.EnrollmentRequestController;
            var courseController = appController.CourseController;
            try
            {
                enrollmentController.CancelRequest(enrollmentRequest, courseController);
                //parentWindow.availableCoursesTab.SetDataForReview();
                parentWindow.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EnrollmentRequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedEnrollmentRequest == null) CancelRequestBtn.IsEnabled = false;
            else CancelRequestBtn.IsEnabled = true;
        }

    }
}
