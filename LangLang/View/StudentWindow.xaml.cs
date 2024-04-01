using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System;
using System.Windows;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public StudentDTO Student { get; set; }
        private StudentController studentController;
        private EnrollmentRequestController enrollmentRequestController;
        private Student currentlyLoggedIn;

        public StudentWindow(StudentController studentController, Student currentlyLoggedIn, EnrollmentRequestController enrollmentRequestController)
        {
            InitializeComponent();
            DataContext = this;
            this.studentController = studentController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            Student = new(this.currentlyLoggedIn);
            gendercb.ItemsSource = Enum.GetValues(typeof(UserGender));
            FillData();
            this.enrollmentRequestController = enrollmentRequestController;
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
            DisableAll();
        }

        private void DisableAll()
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

        private void EnableAll()
        {
            nametb.IsEnabled = true;
            lastnametb.IsEnabled = true;
            emailtb.IsEnabled = true;
            numbertb.IsEnabled = true;
            gendercb.IsEnabled = true;
            passwordtb.IsEnabled = true;
            birthdp.IsEnabled = true;
            professiontb.IsEnabled = true;
        }

        private void signoutbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            if (studentController.CanModifyInfo(currentlyLoggedIn.Id, enrollmentRequestController))
            {
                EnableAll();
            }
            else
            {
                MessageBox.Show("You cannot modify your data as you have registered to attend a course or an exam.");
            }

        }
    }
}
