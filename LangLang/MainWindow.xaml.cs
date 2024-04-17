using LangLang.Core.Controller;
using System.Windows;
using LangLang.View;
using LangLang.Core.Model;
using System.Collections.Generic;
using System.Linq;
using LangLang.View.StudentGUI;
using System.Security.Authentication;

namespace LangLang
{
    public partial class MainWindow : Window
    {

        private AppController appController { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            appController = new();
        }

        private void loginbtn_Click(object sender, RoutedEventArgs e)
        {
            string enteredEmail = emailtb.Text;
            string enteredPassword = passwordtb.Text;

            TrySignUp(enteredEmail, enteredPassword);
        }

        private void signupbtn_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new(appController, appController.StudentController);
            registrationWindow.Show();
            this.Close();
        }

        private void TrySignUp(string email, string password)
        {
            try
            {
                Profile profile = appController.LoginController.GetProfileByCredentials(email, password);
                OpenAppropriateWindow(profile);
                this.Close();
            } 
            catch (AuthenticationException ex) {
                errortb.Text = ex.Message;
            }
        }

        private void OpenAppropriateWindow(Profile profile)
        {
            if (profile.Role == UserType.Student)
            {
                StudentWindow studentWindow = new(appController, appController.StudentController, appController.StudentController.GetAllStudents()[profile.Id], appController.EnrollmentRequestController, appController.CourseController, appController.ExamSlotController);
                studentWindow.Show();
            } else if (profile.Role == UserType.Tutor)
            {
                TutorWindow tutorWindow = new(appController.TutorController.GetAllTutors()[profile.Id]);
                tutorWindow.Show();
            }
        }

    }
}
