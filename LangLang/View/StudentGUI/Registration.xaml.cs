using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System;
using System.Windows;
namespace LangLang.View
{
    public partial class Registration : Window
    {
        public StudentDTO Student { get; set; }
        private StudentController studentController;
        private AppController appController;

        public Registration(AppController appController)
        {
            InitializeComponent();
            DataContext = this;
            Student = new StudentDTO();
            this.studentController = appController.StudentController;
            this.appController = appController;
            gendercb.ItemsSource = Enum.GetValues(typeof(UserGender));
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (Student.IsValid)
            {
                if (appController.EmailExists(Student.Email)) MessageBox.Show("Email already exists. Try with a different email address.");
                else
                {
                    studentController.Add(Student.ToStudent());
                    MessageBox.Show("Success!");
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Something went wrong. Please check all fields in registration form.");
            }
        }

    }
}
