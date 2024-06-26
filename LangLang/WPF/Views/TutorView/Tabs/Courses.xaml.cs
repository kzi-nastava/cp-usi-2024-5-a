﻿using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.Views.TutorView.AdditionalWindows.CourseView;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.TutorView.Tabs
{
    public partial class Courses : UserControl
    {
        public CoursesTutorViewModel CoursesViewModel { get; set; }
        private int tutorId {  get; set; }
        public Courses(Tutor loggedIn)
        {
            InitializeComponent();
            CoursesViewModel = new(loggedIn, this);
            DataContext = CoursesViewModel;
            DisableButtons();
            CoursesViewModel.Update();
            tutorId = loggedIn.Id;
        }

        private void CoursesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CoursesViewModel.SelectedCourse == null)
            {
                DisableButtons();
            }
            else
            {
                EnableButtons();
            }
        }
        private void DisableButtons()
        {
            courseUpdateBtn.IsEnabled = false;
            courseDeleteBtn.IsEnabled = false;
            courseEnrollmentBtn.IsEnabled = false;
            courseEnterGradeBtn.IsEnabled = false;
            courseDurationBtn.IsEnabled = false;
        }

        private void EnableButtons()
        {
            courseUpdateBtn.IsEnabled = true;
            courseDeleteBtn.IsEnabled = true;
            courseEnrollmentBtn.IsEnabled = true;
            courseEnterGradeBtn.IsEnabled = true;
            courseDurationBtn.IsEnabled = true;
        }
        private void CourseCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseCreateWindow createWindow = new(this, tutorId);
            createWindow.Show();
        }

        private void CourseSearchWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseSearchWindow searchWindow = new(CoursesViewModel.LoggedIn);
            searchWindow.Show();
        }


        private void CourseUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CoursesViewModel.UpdateCourse();
        }

        private void CourseDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            CoursesViewModel.DeleteCourse();
        }

        private void EnterGradeBtn_Click(object sender, RoutedEventArgs e)
        {
            CoursesViewModel.EnterGrade();
        }
        
        private void DurationOfCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            CoursesViewModel.DurationOfCurse();
        }

        private void CourseEnrollmentBtn_Click(object sender, RoutedEventArgs e)
        {
            CoursesViewModel.CourseEnrollments();
        }

        public void Update()
        {
            CoursesViewModel.Update();
        }
    }
}
