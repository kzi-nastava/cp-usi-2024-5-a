using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Observer;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.View
{
    public partial class StudentWindow : Window, IObserver
    {
        public StudentDTO Student { get; set; }
        public CoursesDTO Course {  get; set; }
        private StudentController studentController;
        private EnrollmentRequestController enrollmentRequestController;
        private CourseController courseController;
        private Student currentlyLoggedIn;
        private ObservableCollection<CoursesDTO> Courses;
        private ObservableCollection<ExamSlotDTO> ExamSlots;
        private Dictionary<int, Course> coursesForReview;

        public StudentWindow(StudentController studentController, Student currentlyLoggedIn, EnrollmentRequestController enrollmentRequestController, CourseController courseController)
        {
            InitializeComponent();
            DataContext = this;
            this.studentController = studentController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            this.courseController = courseController;
            this.enrollmentRequestController = enrollmentRequestController;
            this.Courses = new ObservableCollection<CoursesDTO>();
            this.ExamSlots = new ObservableCollection<ExamSlotDTO>();
            Student = new(this.currentlyLoggedIn);
            coursesForReview = this.studentController.GetAvailableCourses(this.courseController);
            gendercb.ItemsSource = Enum.GetValues(typeof(UserGender));
            FillData();

            this.enrollmentRequestController = enrollmentRequestController;
        }

        public void Update()
        {
            Courses.Clear();
            foreach (Course course in coursesForReview.Values)
            {
                Courses.Add(new CoursesDTO(course));
            }
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
                EditMode();
            }
            else
            {
                MessageBox.Show("You cannot modify your data as you have registered to attend a course or an exam.");
            }

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (Student.IsValid)
            {
                MessageBox.Show("Success.");
                NormalMode();
                DisableAll();
            } else
            {
                MessageBox.Show("Something went wrong. Please check all fields in registration form.");
            }
        }

        private void discard_Click(object sender, RoutedEventArgs e)
        {
            Student = new(currentlyLoggedIn);
            FillData();
            NormalMode();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete your account?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                studentController.Delete(currentlyLoggedIn.Id, enrollmentRequestController);
                MessageBox.Show("Account is deactivated. All exams and courses have been canceled.");
                this.Close();
            }

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //coursesForReview = this.studentController.Search(); 
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            coursesForReview = this.studentController.GetAvailableCourses(this.courseController);
        }

        private void SearchExam_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearExam_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
