﻿using LangLang.WPF.ViewModels.ExamViewModels;
using LangLang.WPF.Views.DirectorView.AdditionalWindows;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.DirectorView.Tabs
{
    public partial class ExamSlotsReview : UserControl
    {
        public ExamSlotsDirectorViewModel ExamsVM { get; set; }
        public ExamSlotsReview()
        {
            InitializeComponent();
            ExamsVM = new ExamSlotsDirectorViewModel();
            DataContext = ExamsVM;

            Update();
        }
        public void Update()
        {
            ExamsVM.SetDataForReview();
        }
        private void ExamSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotCreateWindow createWindow = new(this);
            createWindow.Show();
        }
    }
}
