using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Observer;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI
{
    public partial class StudentWindow : Window, IObserver
    {
        public StudentDTO Student { get; set; }
        private AppController appController;
        private StudentController studentController;
        private EnrollmentRequestController ERController;
        private WithdrawalRequestController WRController;
        private CourseController courseController;
        private ExamSlotController examSlotController;
        private ExamAppRequestController examAppRequestController;
        private Student currentlyLoggedIn;
        private ObservableCollection<CourseDTO> courses;
        private ObservableCollection<ExamSlotDTO> examSlots;
        private List<Course> coursesForReview;
        private List<ExamSlot> examSlotsForReview;
        private int enrollmentRequestId; // id of enrollment request to current active course

        public StudentWindow(AppController appController, Profile currentlyLoggedIn)
        {
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            this.studentController = appController.StudentController;
            this.currentlyLoggedIn = studentController.GetById(currentlyLoggedIn.Id);
            this.courseController = appController.CourseController;
            this.ERController = appController.EnrollmentRequestController;
            this.WRController = appController.WithdrawalRequestController;
            this.examSlotController = appController.ExamSlotController;
            this.examAppRequestController = appController.ExamAppRequestController;

            this.courses = new ObservableCollection<CourseDTO>();
            this.examSlots = new ObservableCollection<ExamSlotDTO>();

            Student = new(this.currentlyLoggedIn);
            examSlotsForReview = this.studentController.GetAvailableExamSlots(this.currentlyLoggedIn, courseController, examSlotController, ERController);
            coursesForReview = this.studentController.GetAvailableCourses(courseController);

            gendercb.ItemsSource = Enum.GetValues(typeof(Gender));
            levelExamcb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            levelCoursecb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            
            this.studentController.Subscribe(this);
            this.courseController.Subscribe(this);
            this.ERController.Subscribe(this);
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
                ExamSlots.Add(new ExamSlotDTO(exam));
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
            FillCourseInfo();
            NormalMode();
            DisableAll();
        }

        private void FillCourseInfo()
        {
            EnrollmentRequest? enrollmentRequest = ERController.GetActiveCourseRequest(Student.Id, courseController);
            if (enrollmentRequest == null)
            {
                untilEndTb.Text = "You are currently not enrolled in any courses. \nYou can request enrollment or wait for the tutor to accept your request.";
                CourseWithdrawalBtn.Visibility = Visibility.Collapsed;
                return;
            }
            
            int erId = enrollmentRequest.Id;
            // disable the withdrawal request button if the student is ineligible to withdraw or has already withdrawn
            if (!ERController.CanRequestWithdrawal(erId) || WRController.AlreadyExists(erId))
                CourseWithdrawalBtn.IsEnabled = false;

            Course activeCourse = courseController.GetById(enrollmentRequest.CourseId);
            enrollmentRequestId = erId; 
            courseNameTb.Text = activeCourse.Language;
            courseLevelTb.Text = activeCourse.Level.ToString();
            string daysUntilEnd = activeCourse.DaysUntilEnd().ToString();
            untilEndTb.Text = daysUntilEnd + " days until the end of course.";
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

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (studentController.CanModifyInfo(currentlyLoggedIn.Id, ERController, courseController, WRController))
            {
                EnableAll();
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
                    DisableAll();
                }
            } else
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
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete your account?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                studentController.Delete(currentlyLoggedIn.Id, ERController, examAppRequestController);
                MessageBox.Show("Account is deactivated. All exams and courses have been canceled.");
                this.Close();
            }
        }

        private void ClearExamBtn_Click(object sender, RoutedEventArgs e)
        {
            examSlotsForReview = this.studentController.GetAvailableExamSlots(currentlyLoggedIn, this.courseController, examSlotController, ERController);
            levelExamcb.SelectedItem = null;
            Update();
        }
        private void ClearCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            coursesForReview = this.studentController.GetAvailableCourses(this.courseController);
            levelCoursecb.SelectedItem = null;
            Update();
        }

        private void CourseWithdrawalBtn_Click(object sender, RoutedEventArgs e)
        {
            WithdrawalRequestWindow wrWindow = new(appController.WithdrawalRequestController, enrollmentRequestId, this);
            wrWindow.Show();
        }

        private void SearchExams(object sender, RoutedEventArgs e)
        {
            string? language = languageExamtb.Text;
            LanguageLevel? level = null;
            if (levelExamcb.SelectedValue != null)
                level = (LanguageLevel)levelExamcb.SelectedValue;
            DateTime examDate = examdatePicker.SelectedDate ?? default;


            examSlotsForReview = this.studentController.SearchExamSlotsByStudent(examSlotController, courseController, ERController, currentlyLoggedIn.Id, examDate, language, level); ;
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
    }
}
