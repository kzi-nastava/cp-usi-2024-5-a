using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.ViewModels.ExamViewModels;
using LangLang.WPF.Views.StudentView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI.Tabs
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
            levelExamcb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        public void Update()
        {
            AvailableExamsVM.SetDataForReview();
        }
        private void SearchExams(object sender, RoutedEventArgs e)
        {
            string? language = languageExamtb.Text;
            LanguageLevel? level = null;
            if (levelExamcb.SelectedValue != null)
                level = (LanguageLevel)levelExamcb.SelectedValue;
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
