
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.StudentViewModels
{
    public class NotificationsViewModel
    {
        private Student currentlyLoggedIn { get; set; }
        public ObservableCollection<MessageDTO> Messages { get; set; }
        public MessageDTO SelectedMessage { get; set; }

        public NotificationsViewModel(Student currentlyLoggedIn) {
            Messages = new();
            this.currentlyLoggedIn = currentlyLoggedIn;
        }
        public void SetDataForReview()
        {
            var messageService = new MessageController();
            List<Message> studentMessages = messageService.GetReceivedMessages(currentlyLoggedIn);
            foreach (Message message in studentMessages)
            {
                Messages.Add(new MessageDTO(message));
            }
        }

        public void ShowMessage()
        {
            if (SelectedMessage == null) return;
            else MessageBox.Show(SelectedMessage.Text, "Message");
        }
    }
}
