﻿using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for ExamSlotCreateWindow.xaml
    /// </summary>
    public partial class ExamSlotCreateWindow : Window
    {
        public List<Course> Skills { get; set; }
        public Course SelectedCourse { get; set; }
        public ExamSlotViewModel ExamSlot { get; set; }
        public ExamSlotCreateWindow (Tutor loggedIn)
        {
            CourseService courseService = new();
            Skills = courseService.GetBySkills(loggedIn);
            SelectedCourse = null;

            ExamSlot = new ExamSlotViewModel();
            ExamSlot.ExamDate = DateTime.Now;
            ExamSlot.Modifiable = true;
            ExamSlot.TutorId = loggedIn.Id;

            InitializeComponent();
            DataContext = this;

        }

        private void examSlotCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotService examSlotService = new();
            if (ExamSlot.IsValid)
            {               
                if (SelectedCourse==null) MessageBox.Show("Must select language and level.");
                else
                {
                    bool added = examSlotService.Add(ExamSlot.ToExamSlot());
                    
                    if (!added) MessageBox.Show("Choose another exam date or time.");
                    else
                    {
                        MessageBox.Show("Exam successfuly created.");
                        Close();
                    }                   
                }              
            }
            else
            {
                
                if (SelectedCourse==null) MessageBox.Show("Must select language and level.");
                else MessageBox.Show("Exam slot can not be created. Not all fields are valid.");
                           
            }
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedCourse!= null)
            {
                SelectedCourse = (Course)CoursesDataGrid.SelectedItem;
                languageTb.Text = SelectedCourse.Language;
                ExamSlot.Language = SelectedCourse.Language;
                levelTb.Text = SelectedCourse.Level.ToString();
                ExamSlot.Level = SelectedCourse.Level;
                //ExamSlot.CourseId = SelectedCourse.Id;
                //CoursesDataGrid.SelectedItem = null;
            }
        }

    }
}
