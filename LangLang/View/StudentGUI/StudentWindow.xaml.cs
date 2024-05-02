﻿using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Model.Enums;
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
        public EnrollmentRequestDTO EnrollmentRequest { get; set; }
        private AppController appController;
        private StudentController studentController;
        private EnrollmentRequestController erController;
        private WithdrawalRequestController wrController;
        private CourseController courseController;
        private ExamSlotController examSlotController;
        private Student currentlyLoggedIn;
        private ObservableCollection<CourseDTO> courses;
        private ObservableCollection<ExamSlotDTO> examSlots;
        private ObservableCollection<EnrollmentRequestDTO> enrollmentRequests;
        private List<Course> coursesForReview;
        private List<ExamSlot> examSlotsForReview;
        private List<EnrollmentRequest> enrollmentRequestsForReview;
        private int enrollmentRequestId; // id of enrollment request to current active course
        public CourseDTO SelectedCourse {  get; set; }

        public StudentWindow(AppController appController, Profile currentlyLoggedIn)
        {
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            SetControllers();

            this.currentlyLoggedIn = studentController.GetById(currentlyLoggedIn.Id);
            Student = new(this.currentlyLoggedIn);

            EnrollmentRequest = new();
            if (!studentController.CanRequestEnroll(currentlyLoggedIn.Id, erController, courseController, wrController))
            {
                SendRequestBtn.IsEnabled = false;
            }

            CreateObservableCollections();
            SetDataForReview();
            SubscribeControllers();
            FillComboBox();
            FillData();
            Update();
        }

        private void CreateObservableCollections()
        {
            courses = new ObservableCollection<CourseDTO>();
            examSlots = new ObservableCollection<ExamSlotDTO>();
            enrollmentRequests = new ObservableCollection<EnrollmentRequestDTO>();
        }

        private void SetDataForReview()
        {
            examSlotsForReview = studentController.GetAvailableExamSlots(currentlyLoggedIn, courseController, examSlotController, erController);
            coursesForReview = studentController.GetAvailableCourses(currentlyLoggedIn.Id, courseController, erController);
            enrollmentRequestsForReview = erController.GetStudentRequests(currentlyLoggedIn.Id);
        }

        private void SetControllers()
        {
            studentController = appController.StudentController;
            courseController = appController.CourseController;
            erController = appController.EnrollmentRequestController;
            wrController = appController.WithdrawalRequestController;
            examSlotController = appController.ExamSlotController;
        }

        private void FillComboBox()
        {
            gendercb.ItemsSource = Enum.GetValues(typeof(Gender));
            levelExamcb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            levelCoursecb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        private void SubscribeControllers()
        {
            studentController.Subscribe(this);
            courseController.Subscribe(this);
            erController.Subscribe(this);
            examSlotController.Subscribe(this);
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

        public ObservableCollection<EnrollmentRequestDTO> EnrollmentRequests
        {
            get { return enrollmentRequests; }
            set { enrollmentRequests = value; }
        }

        public void Update()
        {   
            Courses.Clear();
            foreach (Course course in coursesForReview)
                Courses.Add(new CourseDTO(course));

            ExamSlots.Clear();
            foreach (ExamSlot exam in examSlotsForReview)
                ExamSlots.Add(new ExamSlotDTO(exam));
            
            EnrollmentRequests.Clear();
            foreach (EnrollmentRequest er in enrollmentRequestsForReview)
                EnrollmentRequests.Add(new EnrollmentRequestDTO(er, appController));
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
            EnrollmentRequest? enrollmentRequest = erController.GetActiveCourseRequest(Student.Id, courseController, wrController);
            if (enrollmentRequest == null)
            {
                untilEndTb.Text = "You are currently not enrolled in any courses. \nYou can request enrollment or wait for the tutor to accept your request.";
                CourseWithdrawalBtn.Visibility = Visibility.Collapsed;
                return;
            }
            
            int erId = enrollmentRequest.Id;
            // disable the withdrawal request button if the student is ineligible to withdraw or has already withdrawn
            if (!erController.CanRequestWithdrawal(erId) || wrController.AlreadyExists(erId))
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
            Close();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (studentController.CanModifyInfo(currentlyLoggedIn.Id, erController, courseController, wrController))
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
                studentController.Delete(currentlyLoggedIn.Id, erController);
                MessageBox.Show("Account is deactivated. All exams and courses have been canceled.");
                this.Close();
            }
        }

        private void ClearExamBtn_Click(object sender, RoutedEventArgs e)
        {
            examSlotsForReview = this.studentController.GetAvailableExamSlots(currentlyLoggedIn, this.courseController, examSlotController, erController);
            levelExamcb.SelectedItem = null;
            Update();
        }
        private void ClearCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            coursesForReview = studentController.GetAvailableCourses(currentlyLoggedIn.Id, courseController, erController);
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


            examSlotsForReview = studentController.SearchExamSlotsByStudent(examSlotController, courseController, erController, currentlyLoggedIn.Id, examDate, language, level); ;
            Update();
        }

        private void SearchCourses(object sender, RoutedEventArgs e)
        {
            string? language = languagetb.Text;
            LanguageLevel? level = null;
            if (levelCoursecb.SelectedValue != null)
                level = (LanguageLevel)levelCoursecb.SelectedValue;
            DateTime courseStartDate = courseStartdp.SelectedDate ?? default;
            int.TryParse(durationtb.Text, out int duration);
            coursesForReview = studentController.SearchCoursesByStudent(currentlyLoggedIn.Id, courseController, erController, language, level, courseStartDate, duration, !onlinecb.IsChecked);
            Update();
        }

        private void SendRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCourse  == null) return;
            EnrollmentRequest.CourseId = SelectedCourse.Id;
            EnrollmentRequest.StudentId = currentlyLoggedIn.Id;
            EnrollmentRequest.Status = Status.Pending;
            EnrollmentRequest.RequestSentAt = DateTime.Now;
            EnrollmentRequest.LastModifiedTimestamp = DateTime.Now;
            EnrollmentRequest.IsCanceled = false;
            erController.Add(EnrollmentRequest.ToEnrollmentRequest());
            MessageBox.Show("Request sent. Please wait for approval.");
            coursesForReview = studentController.GetAvailableCourses(currentlyLoggedIn.Id, courseController, erController);
            Update();
        }
    }
}
