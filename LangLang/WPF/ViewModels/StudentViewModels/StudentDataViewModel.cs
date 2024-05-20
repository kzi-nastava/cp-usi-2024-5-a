using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using System.Windows;

namespace LangLang.WPF.ViewModels.StudentViewModels
{
    public class StudentDataViewModel
    {
        public StudentViewModel Student { get; set; }
        private Student currentlyLoggedIn;

        public StudentDataViewModel(Student currentlyLoggedIn)
        {
            this.currentlyLoggedIn = currentlyLoggedIn;
            Student = new(currentlyLoggedIn);
        }

        public bool Save()
        {
            var profileService = new ProfileService();

            if (!Student.IsValid)
            {
                MessageBox.Show("Something went wrong. Please check all fields in registration form.");
                return false;
            }

            if (profileService.EmailExists(Student.Email, Student.Id, UserType.Student))
            {
                MessageBox.Show("Email already exists. Try with a different email address.");
                return false;
            }

            var studentService = new StudentService();
            studentService.Update(Student.ToStudent());
            MessageBox.Show("Success.");
            return true;
        }

        public bool Edit()
        {
            var studentService = new StudentService();
            if (!studentService.CanModifyData(currentlyLoggedIn))
            {
                MessageBox.Show("You cannot modify your data as you have registered to attend a course or an exam.");
                return false;
            }
            return true;
        }

        public void DiscardDataChanges()
        {
            Student = new(currentlyLoggedIn);
        }

        public void Delete()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete your account?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            var studentService = new StudentService();
            studentService.Deactivate(currentlyLoggedIn.Id);
            MessageBox.Show("Account is deactivated. All exams and courses have been canceled.");

        }
    }
}
