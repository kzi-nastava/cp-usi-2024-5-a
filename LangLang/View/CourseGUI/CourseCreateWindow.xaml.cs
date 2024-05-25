﻿using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.Domain.Models.Enums;
using LangLang.WPF.ViewModels.CourseViewModels;
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
        public CourseViewModel Course { get; set; }
        public CourseCreateWindow(Tutor loggedIn)
        {
            Course = new CourseViewModel();
            Course.TutorId = loggedIn.Id;
            InitializeComponent();
            DataContext = this;
            languageLvlCb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            classsroomCb.IsChecked = false;
            maxNumOfStudentsTb.IsEnabled = false;
            mon.IsChecked = false;
            tue.IsChecked = false;
            wed.IsChecked = false;
            thu.IsChecked = false;
            fri.IsChecked = false;
            startDateDp.SelectedDate = DateTime.Now;
        }

        private void CourseCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Course.IsValid)
            {
                var courseService = new CourseService();
                if (courseService.CanCreateOrUpdate(Course.ToCourse()))
                {
                    courseService.Add(Course.ToCourse());
                    MessageBox.Show("Success!");
                    Close();
                }
                else
                {
                    MessageBox.Show("The course cannot be created, there are time overlaps or no available classroms (if the course is held in a classroom).");
                }
            }
            else
            {
                MessageBox.Show("Something went wrong. Please check all fields in the form.");
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
    }
}
