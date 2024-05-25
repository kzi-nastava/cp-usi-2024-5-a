using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
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
    public partial class ExamApplications : UserControl
    {
        public ExamApplicationsVM ApplicationsVM { get; set; }

        public ExamApplications(Student loggedIn, StudentWindow parent)
        {
            InitializeComponent();
            DataContext = this;

            ApplicationsVM = new(loggedIn, parent);

            DataContext = ApplicationsVM;
            Update();
        }

        
        public void Update()
        {
            ApplicationsVM.Update();
        }

        private void CancelApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            ApplicationsVM.CancelApplication();
            
        }
    }
}
