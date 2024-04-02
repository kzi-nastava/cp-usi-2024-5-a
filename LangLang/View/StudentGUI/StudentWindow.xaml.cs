using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Observer;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI
{
    public partial class StudentWindow : Window, IObserver
    {
        public StudentDTO Student { get; set; }
        private StudentController studentController;
        private EnrollmentRequestController enrollmentRequestController;
        private CourseController courseController;
        private ExamSlotController examSlotController;
        private AppController appController;
        private Student currentlyLoggedIn;
        private ObservableCollection<CourseDTO> courses;
        private ObservableCollection<ExamSlotDTO> examSlots;
        private List<Course> coursesForReview;
        private List<ExamSlot> examSlotsForReview;

        public StudentWindow(AppController appController, StudentController studentController, Student currentlyLoggedIn, EnrollmentRequestController enrollmentRequestController, CourseController courseController, ExamSlotController examSlotController)
        {
            InitializeComponent();
            DataContext = this;

            this.studentController = studentController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            this.courseController = courseController;
            this.enrollmentRequestController = enrollmentRequestController;
            this.examSlotController = examSlotController;
            this.appController = appController;

            this.courses = new ObservableCollection<CourseDTO>();
            this.examSlots = new ObservableCollection<ExamSlotDTO>();

            Student = new(this.currentlyLoggedIn);
            examSlotsForReview = this.studentController.GetAvailableExamSlots(currentlyLoggedIn, courseController, examSlotController, enrollmentRequestController);
            coursesForReview = this.studentController.GetAvailableCourses(courseController);

            gendercb.ItemsSource = Enum.GetValues(typeof(UserGender));
            levelExamcb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            levelCoursecb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            
            this.studentController.Subscribe(this);
            this.courseController.Subscribe(this);
            this.enrollmentRequestController.Subscribe(this);
            this.examSlotController.Subscribe(this);

            FillData();
            Update();
        }

        public ObservableCollection<CourseDTO> Courses
        {
            get { return courses; }
            set { courses = value; }
        }

        public ObservableCollection<ExamSlotDTO> ExamSlots
        {
            get { return examSlots; }
            set { examSlots = value; }
        }


        public void Update()
        {
            Courses.Clear();
            foreach (Course course in coursesForReview)
            {
                Courses.Add(new CourseDTO(course));
            }
            ExamSlots.Clear();
            foreach (ExamSlot exam in examSlotsForReview)
            {
                Course c = courseController.GetAllCourses()[exam.CourseId];
                ExamSlots.Add(new ExamSlotDTO(exam, c));
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
                if (appController.EmailExists(Student.Email, Student.Id, UserType.Student)) MessageBox.Show("Email already exists. Try with a different email address.");
                else
                {
                    studentController.Update(Student.ToStudent());
                    MessageBox.Show("Success.");
                    NormalMode();
                    DisableAll();
                }
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

        private void SearchExams(object sender, RoutedEventArgs e)
        {
            string? language = languageExamtb.Text;
            LanguageLevel? level = null;
            if (levelExamcb.SelectedValue != null)
                level = (LanguageLevel)levelExamcb.SelectedValue;
            DateTime examDate = examdatePicker.SelectedDate ?? default;


            examSlotsForReview = this.studentController.SearchExamSlotsByStudent(examSlotController, courseController, enrollmentRequestController, currentlyLoggedIn.Id, examDate, language, level); ;
            Update();
        }

        private void SearchCourses(object sender, RoutedEventArgs e)
        {
            string? language = languagetb.Text;
            LanguageLevel? level = null;
            if (levelCoursecb.SelectedValue != null)
                level = (LanguageLevel)levelCoursecb.SelectedValue;
            DateTime courseStartDate = courseStartdp.SelectedDate ?? default;
            int duration = 0;
            int.TryParse(durationtb.Text, out duration);
            coursesForReview = this.studentController.SearchCoursesByStudent(courseController, language, level, courseStartDate, duration, !onlinecb.IsChecked);
            Update();
        }

        private void ClearExam_Click(object sender, RoutedEventArgs e)
        {
            examSlotsForReview = this.studentController.GetAvailableExamSlots(currentlyLoggedIn, this.courseController, examSlotController, enrollmentRequestController);
            levelExamcb.SelectedItem = null;
            Update();
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            coursesForReview = this.studentController.GetAvailableCourses(this.courseController);
            levelCoursecb.SelectedItem = null;
            Update();
        }

    }
}
