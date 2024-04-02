﻿using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for CourseUpdateWindow.xaml
    /// </summary>
    public partial class CourseUpdateWindow : Window
    {
        public CourseDTO Course { get; set; }
        private CourseController courseController;
        public CourseUpdateWindow(CourseController courseControler, int courseId)
        {
            this.courseController = courseControler;
            Course = new CourseDTO(courseController.GetById(courseId));
            InitializeComponent();
            DataContext = this;
            languageLvlCb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        private void CourseUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Course.IsValid)
            {
                courseController.Update(Course.ToCourse());
                MessageBox.Show("Success!");
                Close();
            }
            else
            {
                MessageBox.Show("Something went wrong. Please check all fields in registration form.");
            }

        }

        // Method enables textbox for maxNumOfStudents when the course is to be held in a classroom

        private void ClasssroomCb_Checked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = true;
        }

        private void ClasssroomCb_Unchecked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = false;
        }
    }
}