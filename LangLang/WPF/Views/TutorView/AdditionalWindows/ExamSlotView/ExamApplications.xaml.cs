﻿using LangLang.WPF.ViewModels.ExamViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.TutorView.AdditionalWindows.ExamSlotView
{
    public partial class ExamApplications : Window
    {   
        public ExamApplicationsPageViewModel ExamApplicationsViewModel { get; set; }
        private ExamSlotViewModel ExamSlot;

        public ExamApplications(ExamSlotViewModel examSlot)
        {
            InitializeComponent();
            ExamApplicationsViewModel = new(examSlot);

            DataContext = ExamApplicationsViewModel;
            ExamSlot = examSlot;

            AdjustButtons();
            Update();
        }

        public void Update()
        {
            ExamApplicationsViewModel.Update();
        }

        private void AdjustButtons()
        {
            if (ExamSlot.Modifiable == false)
                confirmApplicationsBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

        private void ConfirmApplicationsBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamApplicationsViewModel.ConfirmApplications();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamApplicationsViewModel.Delete();
        }

        private void ApplicationTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExamApplicationsViewModel.SelectedApplication != null)
                deleteBtn.IsEnabled = true;
            else
                deleteBtn.IsEnabled = false;
        }

    }
}
