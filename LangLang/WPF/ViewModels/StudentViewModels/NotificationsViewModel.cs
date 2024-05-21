
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.StudentViewModels
{
     // Implement once the email sending functionality is added.
    public class NotificationsViewModel
    {
        private Student currentlyLoggedIn { get; set; }

        public NotificationsViewModel(Student currentlyLoggedIn) {
            this.currentlyLoggedIn = currentlyLoggedIn;
        }
        public void SetDataForReview()
        {
            
        }

        public void ShowMessage()
        {

        }
    }
}
