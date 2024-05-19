using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.DTO;
using LangLang.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.RequestsViewModel
{
    public class EnrollmentRequestsViewModel
    {
        public EnrollmentRequestDTO SelectedEnrollmentRequest { get; set; }
        private Student currentlyLoggedIn { get; set; }
        private StudentWindow parentWindow { get; set; }
        public ObservableCollection<EnrollmentRequestDTO> StudentRequests { get; set; }
        public List<EnrollmentRequest> RequestsForReview { get; set; }
        public EnrollmentRequestsViewModel(Student currentlyLoggedIn, StudentWindow studentWindow)
        {
            this.currentlyLoggedIn = currentlyLoggedIn;
            parentWindow = studentWindow;
            StudentRequests = new();
            SetDataForReview();
        }

        public void SetDataForReview()
        {
            var enrollmentService = new EnrollmentRequestController();
            RequestsForReview = enrollmentService.GetRequests(currentlyLoggedIn);
        }

        public void CancelRequest()
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

            var enrollmentService = new EnrollmentRequestController();
            try
            {
                enrollmentService.CancelRequest(enrollmentRequest);
                parentWindow.AvailableCoursesTab.SetDataForReview();
                SetDataForReview();
                parentWindow.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool EnableButton()
        {
            return SelectedEnrollmentRequest != null;
        }

        public void Update()
        {
            StudentRequests.Clear();
            foreach (EnrollmentRequest er in RequestsForReview)
                StudentRequests.Add(new EnrollmentRequestDTO(er));
        }
    }
}
