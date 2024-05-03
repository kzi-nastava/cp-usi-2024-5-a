using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI.Tabs
{
    public partial class StudentData : UserControl
    {
        private AppController appController {  get; set; }
        private Student currentlyLoggedIn {  get; set; }
        private StudentDTO Student {  get; set; }
        private StudentController studentController { get; set; }
        private StudentWindow parentWindow {  get; set; }

        public StudentData(AppController appController, Student currentlyLoggedIn, StudentWindow studentWindow)
        {
            InitializeComponent();
            this.appController = appController;
            studentController = appController.StudentController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            Student = new(currentlyLoggedIn);
            parentWindow = studentWindow;
            FillData();
            DisableForm();
        }

        private void EnableForm()
        {
            nametb.IsEnabled = true;
            lastnametb.IsEnabled = true;
            emailtb.IsEnabled = true;
            numbertb.IsEnabled = true;
            gendercb.IsEnabled = true;
            passwordtb.IsEnabled = true;
            birthdp.IsEnabled = true;
            professiontb.IsEnabled = true;
            gendercb.ItemsSource = Enum.GetValues(typeof(Gender));
        }

        private void DisableForm()
        {
            nametb.IsEnabled = false;
            lastnametb.IsEnabled = false;
            emailtb.IsEnabled = false;
            numbertb.IsEnabled = false;
            gendercb.IsEnabled = false;
            passwordtb.IsEnabled = false;
            birthdp.IsEnabled = false;
            professiontb.IsEnabled = false;
        }


        private void EditMode()
        {
            savebtn.Visibility = Visibility.Visible;
            discardbtn.Visibility = Visibility.Visible;
        }

        private void NormalMode()
        {
            savebtn.Visibility = Visibility.Collapsed;
            discardbtn.Visibility = Visibility.Collapsed;
        }

        private void FillData()
        {
            currenttb.Text = Student.Name + " " + Student.LastName;
            nametb.Text = Student.Name;
            lastnametb.Text = Student.LastName;
            emailtb.Text = Student.Email;
            numbertb.Text = Student.PhoneNumber;
            gendercb.SelectedItem = Student.Gender;
            passwordtb.Text = Student.Password;
            birthdp.SelectedDate = Student.BirthDate;
            professiontb.Text = Student.Profession;
            NormalMode();
            DisableForm();
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var enrollmentController = appController.EnrollmentRequestController;
            var courseController = appController.CourseController;
            var withdrawalController = appController.WithdrawalRequestController;
            if (studentController.CanModifyInfo(currentlyLoggedIn.Id, enrollmentController, courseController, withdrawalController))
            {
                EnableForm();
                EditMode();
            }
            else
            {
                MessageBox.Show("You cannot modify your data as you have registered to attend a course or an exam.");
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Student.IsValid)
            {
                if (appController.EmailExists(Student.Email, Student.Id, UserType.Student)) MessageBox.Show("Email already exists. Try with a different email address.");
                else
                {
                    studentController.Update(Student.ToStudent());
                    MessageBox.Show("Success.");
                    NormalMode();
                    DisableForm();
                }
            }
            else
            {
                MessageBox.Show("Something went wrong. Please check all fields in registration form.");
            }
        }

        private void DiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            Student = new(currentlyLoggedIn);
            FillData();
            NormalMode();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var enrollmentController = appController.EnrollmentRequestController;
            var applicationController = appController.ExamAppRequestController;
            var examController = appController.ExamSlotController;
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete your account?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                studentController.Delete(currentlyLoggedIn.Id, enrollmentController, applicationController, examController);
                MessageBox.Show("Account is deactivated. All exams and courses have been canceled.");
                parentWindow.Close();
            }
        }

    }
}
