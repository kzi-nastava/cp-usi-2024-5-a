﻿using LangLang.Domain.Enums;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.Views.DirectorView.Tabs;
using System;
using System.Windows;

namespace LangLang.WPF.Views.DirectorView.AdditionalWindows
{
    public partial class CourseCreateWindow : Window
    {
        public CreateCourseDirectorVM CreateCourseVM { get; set; }
        private CoursesReview _parent;
        public CourseCreateWindow(CoursesReview parent)
        {
            InitializeComponent();
            _parent = parent;
            CreateCourseVM = new();
            DataContext = CreateCourseVM;
            SetUpForm();
        }

        private void CourseCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CreateCourseVM.CreatedCourse())
            {
                _parent.Update();
                Close();
            }
        }

        // Method enables textbox for maxNumOfStudents when the course is to be held in a classroom
        private void ClasssroomCb_Checked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = true;
            inClassroomErrorTb.Text = "Please enter the maximal number of students, as it is required";
        }

        private void ClasssroomCb_Unchecked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = false;
            inClassroomErrorTb.Text = "";
        }

        private void SetUpForm()
        {
            languageLvlCb.ItemsSource = Enum.GetValues(typeof(Level));
            classsroomCb.IsChecked = false;
            maxNumOfStudentsTb.IsEnabled = false;
            mon.IsChecked = false;
            tue.IsChecked = false;
            wed.IsChecked = false;
            thu.IsChecked = false;
            fri.IsChecked = false;
            startDateDp.SelectedDate = DateTime.Now;
        }
    }
}
