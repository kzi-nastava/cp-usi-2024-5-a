using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI.Tabs
{
    public partial class Notifications : UserControl
    {
        private StudentWindow parentWindow { get; set; }
        private AppController appController { get; set; }
        private Student currentlyLoggedIn { get; set; }
        public ObservableCollection<MessageDTO> Messages { get; set; }
        public MessageDTO SelectedMessage {  get; set; }
        public Notifications(AppController appController, Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            DataContext = this;
            Messages = new();
            this.appController = appController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            this.parentWindow = parentWindow;
            SetDataForReview();
        }

        private void SetDataForReview()
        {
            var messageController = appController.MessageController;
            List<Message> studentMessages = messageController.GetReceivedMessages(currentlyLoggedIn);
            foreach (Message message in studentMessages)
            {
                Messages.Add(new MessageDTO(message, appController));
            }
        }

        private void MessagesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedMessage == null) return;
            else MessageBox.Show(SelectedMessage.Text, "Message");
        }
    }
}
