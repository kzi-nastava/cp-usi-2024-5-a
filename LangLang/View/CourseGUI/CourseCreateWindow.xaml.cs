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
    /// Interaction logic for CourseCreateWindow.xaml
    /// </summary>
    public partial class CourseCreateWindow : Window
    {
        public CourseDTO Course { get; set; }
        private CourseController courseController;
        public CourseCreateWindow(CourseController courseController)
        {
            Course = new CourseDTO();
            this.courseController = courseController;
            InitializeComponent();
            DataContext = this;
            languageLvlCb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            maxNumOfStudentsTb.IsEnabled = false;
        }

        private void CourseCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            if (mon.IsChecked == true)
            {
                days.Add(DayOfWeek.Monday);
            }
            if (tue.IsChecked == true)
            {
                days.Add(DayOfWeek.Tuesday);
            }
            if (wed.IsChecked == true)
            {
                days.Add(DayOfWeek.Wednesday);
            }
            if (thu.IsChecked == true)
            {
                days.Add(DayOfWeek.Thursday);
            }
            if (fri.IsChecked == true)
            {
                days.Add(DayOfWeek.Friday);
            }
            if (days.Count != 0)
            {
                Course.Days = days;
                Course.Online = classsroomCb.IsChecked == false;
                if (Course.IsValid)
                {
                    courseController.Add(Course.ToCourse());
                    MessageBox.Show("Success!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Something went wrong. Please check all fields in registration form.");
                }
            }
            else
            {
                MessageBox.Show("At least one day must be checked.");
            }

        }

        // Method enables textbox for maxNumOfStudents when the course is to be held in a classroom

        private void ClasssroomCb_Checked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = true;
            Course.ErrorStringForMaxStudents = "";
        }

        private void ClasssroomCb_Unchecked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = false;
            Course.ErrorStringForMaxStudents = "Maximal number of students must be a positive number";
        }
    }
}
