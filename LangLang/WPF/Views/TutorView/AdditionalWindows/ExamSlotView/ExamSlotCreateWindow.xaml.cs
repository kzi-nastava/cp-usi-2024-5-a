﻿using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModels;
using LangLang.WPF.Views.TutorView.Tabs;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.TutorView.AdditionalWindows.ExamSlotView
{
    public partial class ExamSlotCreateWindow : Window
    {
        public ExamSlotCreateVM ExamCreateVM { get; set; }
        private ExamsReview _parent;
        public ExamSlotCreateWindow (Tutor loggedIn, ExamsReview parent)
        {
            InitializeComponent();
            _parent = parent;
            ExamCreateVM = new ExamSlotCreateVM(loggedIn);
            DataContext = ExamCreateVM;

        }

        private void examSlotCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ExamCreateVM.CreateExam())
            {
                _parent.Update();
                Close();
            }
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExamCreateVM.SelectedCourse == null) return;
            
            languageTb.Text = ExamCreateVM.SelectedCourse.Language;
            ExamCreateVM.ExamSlot.Language = ExamCreateVM.SelectedCourse.Language;
            levelTb.Text = ExamCreateVM.SelectedCourse.Level.ToString();
            ExamCreateVM.ExamSlot.Level = ExamCreateVM.SelectedCourse.Level;
            
        }

    }
}
