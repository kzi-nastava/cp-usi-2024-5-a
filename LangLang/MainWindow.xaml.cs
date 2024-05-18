using LangLang.Core.Controller;
using System.Windows;
using LangLang.View;
using LangLang.Core.Model;
using System.Security.Authentication;
using LangLang.Domain.Models;
using LangLang.WPF.Views;

namespace LangLang
{
    public partial class MainWindow : Window
    {
        private AppController appController { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            appController = new();
            PenaltyPointReducer reducer = new PenaltyPointReducer();
            reducer.UpdatePenaltyPoints(appController);
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string enteredEmail = emailtb.Text;
            string enteredPassword = passwordtb.Text;

            TrySignUp(enteredEmail, enteredPassword);
        }

        private void SignupBtn_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new();
            registrationWindow.Show();
            Close();
        }
        
        private void TrySignUp(string email, string password)
        {
            try
            {
                Profile profile = appController.LoginController.GetProfileByCredentials(email, password);
                OpenAppropriateWindow(profile);
                Close();
            } 
            catch (AuthenticationException ex) {
                errortb.Text = ex.Message;
            }
        }

        private void OpenAppropriateWindow(Profile profile)
        {
            if (profile.Role == UserType.Student)
            {
                StudentWindow studentWindow = new(profile);
                studentWindow.Show();
            } else if (profile.Role == UserType.Tutor)
            {
                TutorWindow tutorWindow = new(appController, profile);
                tutorWindow.Show();
            } else if (profile.Role == UserType.Director)
            {
                DirectorWindow directorWindow = new(appController);
                directorWindow.Show();
            }
        }

    }
}
