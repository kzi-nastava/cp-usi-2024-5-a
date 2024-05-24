
using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.WPF.Views.DirectorView;
using LangLang.WPF.Views.StudentView;
using System.ComponentModel;
using System.Security.Authentication;

namespace LangLang.WPF.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        private string _email = "";
        private string _password = "";
        private string _error = "";

        public string Email
        {
            get { return _email; }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged("Email");
                }
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        public string Error
        {
            get { return _error; }
            set
            {
                if (value != _error)
                {
                    _error = value;
                    OnPropertyChanged("Error");
                }
            }
        }

        public MainWindowViewModel() {}
        public bool Login()
        {
            return TrySignUp(Email, Password);
        }

        private bool TrySignUp(string email, string password)
        {
            try
            {
                var loginService = new LoginService();
                Profile profile = loginService.GetProfileByCredentials(email, password);
                OpenAppropriateWindow(profile);
                return true;
            }
            catch (AuthenticationException ex)
            {
                Error = ex.Message;
                return false;
            }
        }

        private void OpenAppropriateWindow(Profile profile)
        {
            if (profile.Role == UserType.Student)
            {
                StudentWindow studentWindow = new(profile);
                studentWindow.Show();
            }
            else if (profile.Role == UserType.Tutor)
            {
                TutorWindow tutorWindow = new(profile);
                tutorWindow.Show();
            }
            else if (profile.Role == UserType.Director)
            {
                DirectorWindow directorWindow = new();
                directorWindow.Show();
            }
        }

        public void ShowRegistrationWindow()
        {
            var registrationWindow = new Registration();
            registrationWindow.Show();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
