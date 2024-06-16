
using System.Windows;
using System;
using LangLang.Domain.Models;
using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;

namespace LangLang.WPF.ViewModels.RequestViewModels
{
    public class WithdrawalReqPageViewModel
    {
        public WithdrawalRequestViewModel WithdrawalRequest { get; set; }
        private readonly int enrollmentRequestId;

        public WithdrawalReqPageViewModel(int enrollmentRequestId)
        {
            WithdrawalRequest = new WithdrawalRequestViewModel();
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
            var withrawalService = new WithdrawalRequestService();
            withrawalService.Add(wr);
            MessageBox.Show("Request sent. Please wait for approval.");
        }
    }
}
