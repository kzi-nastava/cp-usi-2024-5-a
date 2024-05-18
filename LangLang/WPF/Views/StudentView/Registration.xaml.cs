using LangLang.Aplication.UseCases;
using LangLang.Core.Model;
using LangLang.WPF.ViewModels.StudentViewModels;
using System;
using System.Windows;
namespace LangLang.View
{
    public partial class Registration : Window
    {
        public StudentViewModel Student { get; set; }

        public Registration()
        {
            InitializeComponent();
            DataContext = this;
            Student = new StudentViewModel();
            gendercb.ItemsSource = Enum.GetValues(typeof(Gender));
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var profileService = new ProfileService();
            var studentService = new StudentService();
            
            if (!Student.IsValid)
            {
                MessageBox.Show("Something went wrong. Please check all fields in registration form.");
                return;
            }

            if (profileService.EmailExists(Student.Email))
            {
                MessageBox.Show("Email already exists. Try with a different email address.");
                return;
            }
            
            studentService.Add(Student.ToStudent());
            MessageBox.Show("Success!");
            Close();
        }

    }
}
