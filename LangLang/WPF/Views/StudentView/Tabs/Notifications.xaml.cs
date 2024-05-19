using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.StudentViewModels;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI.Tabs
{
    public partial class Notifications : UserControl
    {
        public NotificationsViewModel notificationsViewModel { get; set; }
        public Notifications(Student currentlyLoggedIn)
        {
            InitializeComponent();
            notificationsViewModel = new(currentlyLoggedIn);
            DataContext = notificationsViewModel;
            SetDataForReview();
        }

        private void SetDataForReview()
        {
            notificationsViewModel.SetDataForReview();
        }

        private void MessagesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            notificationsViewModel.ShowMessage();
        }
    }
}
