using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using LangLang.DTO;
using System;
using System.Windows;

namespace LangLang.View.StudentGUI
{
    public partial class WithdrawalRequestWindow : Window
    {
        public WithdrawalRequestDTO WithdrawalRequest { get; set; }
        public WithdrawalRequestController WRController { get; set; }
        private readonly StudentWindow parentWindow;
        private readonly int enrollmentRequestId;

        public WithdrawalRequestWindow(WithdrawalRequestController wrController, int enrollmentRequestId, StudentWindow parentWindow)
        {
            InitializeComponent();
            DataContext = this;
            WithdrawalRequest = new WithdrawalRequestDTO();
            WRController = wrController;
            this.parentWindow = parentWindow;
            this.enrollmentRequestId = enrollmentRequestId;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            WithdrawalRequest wr = WithdrawalRequest.ToWithdrawalRequest();
            if (string.IsNullOrEmpty(wr.Reason))
            {
                MessageBox.Show("Please enter a reason for leaving the course." +
                    "\nWithout providing a reason, we will not be able to process your request." +
                    "\nThank you for your understanding!");
                return;
            }
            wr.RequestSentAt = DateTime.Now;
            wr.UpdateStatus(Status.Pending);
            wr.EnrollmentRequestId = enrollmentRequestId;
            WRController.Add(wr);
            MessageBox.Show("Request sent. Please wait for approval.");

            Close();
        }


        
    }
}
