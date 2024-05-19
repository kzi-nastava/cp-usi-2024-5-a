
using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using LangLang.Core.Model;
using LangLang.DTO;
using System.Windows;
using System;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class WithdrawalReqPageViewModel
    {
        public WithdrawalRequestDTO WithdrawalRequest { get; set; }
        private readonly int enrollmentRequestId;

        public WithdrawalReqPageViewModel(int enrollmentRequestId)
        {
            WithdrawalRequest = new WithdrawalRequestDTO();
            this.enrollmentRequestId = enrollmentRequestId;
        }

        public void Submit()
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
            var withrawalService = new WithdrawalRequestController();
            withrawalService.Add(wr);
            MessageBox.Show("Request sent. Please wait for approval.");
        }
    }
}
