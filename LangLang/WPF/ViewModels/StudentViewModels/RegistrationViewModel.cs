
using LangLang.BusinessLogic.UseCases;
using System.Windows;

namespace LangLang.WPF.ViewModels.StudentViewModels
{
    public class RegistrationViewModel
    {
        public StudentViewModel Student { get; set; }

        public RegistrationViewModel()
        {
            Student = new StudentViewModel();
        }

        public bool SignUp()
        {
            var profileService = new ProfileService();
            var studentService = new StudentService();

            if (!Student.IsValid)
            {
                MessageBox.Show("Something went wrong. Please check all fields in registration form.");
                return false;
            }

            if (profileService.EmailExists(Student.Email))
            {
                MessageBox.Show("Email already exists. Try with a different email address.");
                return false;
            }

            studentService.Add(Student.ToStudent());
            MessageBox.Show("Success!");
            return true;
        }

    }
}
