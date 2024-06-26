﻿using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.StudentView.Tabs
{
    public partial class AvailableExams : UserControl
    {
        public AvailableExamsVM AvailableExamsVM { get; set; }

        public AvailableExams( Student loggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            AvailableExamsVM = new(loggedIn,parentWindow);
            DataContext = AvailableExamsVM;
            Update();
            levelExamcb.ItemsSource = Enum.GetValues(typeof(Level));
        }

        public void Update()
        {
            AvailableExamsVM.SetDataForReview();
            AvailableExamsVM.Update();
        }
        private void SearchExams(object sender, RoutedEventArgs e)
        {
            string? language = languageExamtb.Text;
            Level? level = null;
            if (levelExamcb.SelectedValue != null)
                level = (Level)levelExamcb.SelectedValue;
            DateTime examDate = examdatePicker.SelectedDate ?? default;
            AvailableExamsVM.SearchExams(AvailableExamsVM.loggedIn, examDate, language, level);
            
        }

        private void ClearExamBtn_Click(object sender, RoutedEventArgs e)
        {
            AvailableExamsVM.ClearExams();
        }
        private void SendApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            AvailableExamsVM.SendApplication();

        }
    }
}
