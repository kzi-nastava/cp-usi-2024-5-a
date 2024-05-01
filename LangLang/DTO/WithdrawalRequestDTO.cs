using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using System;
using System.ComponentModel;

namespace LangLang.DTO
{
    public class WithdrawalRequestDTO : INotifyPropertyChanged
    {   
        public int Id { get; set; }
        private int enrollmentRequestId;
        private string reason;
        private Status status;
        private DateTime requestSentAt;
        private DateTime requestReceivedAt;

        public string Reason
        {
            get
            {
                return reason;
            }
            set
            {
                if (value != reason)
                {
                    reason = value;
                    OnPropertyChanged("Reason");
                }
            }
        }

        public WithdrawalRequestDTO() {}

        public WithdrawalRequestDTO(WithdrawalRequest request, AppController appController)
        {
            Id = request.Id;
            enrollmentRequestId = request.EnrollmentRequestId;
            reason = request.Reason;
            status = request.Status;
            requestSentAt = request.RequestSentAt;
            requestReceivedAt = request.RequestReceivedAt;
        }

        public WithdrawalRequest ToWithdrawalRequest()
        {
            return new WithdrawalRequest(Id, enrollmentRequestId, reason, status, requestSentAt, requestReceivedAt);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
