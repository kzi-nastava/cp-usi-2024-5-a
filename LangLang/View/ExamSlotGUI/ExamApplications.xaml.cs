﻿using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.ExamSlotGUI
{
    /// <summary>
    /// Interaction logic for ExamApplications.xaml
    /// </summary>
    public partial class ExamApplications : Window
    {
        public ExamAppRequestDTO SelectedApplication { get; set; }
        public ObservableCollection<ExamAppRequestDTO> Applications { get; set; }
        private AppController appController;
        private ExamAppRequestController applicationController;
        private ExamSlotController examSlotController;
        private StudentController studentController;
        private ExamSlotDTO examSlot;

        public ExamApplications(AppController appController, ExamSlotDTO examSlot)
        {
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            this.examSlot = examSlot;
            applicationController = appController.ExamAppRequestController;
            examSlotController = appController.ExamSlotController;
            studentController = appController.StudentController;

            Applications = new();

            AdjustButtons();

            Update();
        }

        public void Update()
        {
            Applications.Clear();
            foreach (ExamAppRequest application in applicationController.GetApplications(examSlot.Id))
            {
                Applications.Add(new ExamAppRequestDTO(application, appController));
            }
        }

        private void AdjustButtons()
        {
            if (examSlot.Modifiable == false)
                confirmApplicationsBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

        private void ConfirmApplicationsBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to confirm list?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) {
                examSlot.Modifiable = false;
                examSlotController.Update(examSlot.ToExamSlot());
                AdjustButtons();
                ShowSuccess();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete " + SelectedApplication.StudentName + " " + SelectedApplication.StudentLastName  + " from the examination?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Student student = studentController.GetById(SelectedApplication.StudentId);
                studentController.Delete(student, appController);
                Update();
                ShowSuccess();
            }
        }

        // NOTE: think about moving to utils
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApplicationTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedApplication != null)
                deleteBtn.IsEnabled = true;
            else
                deleteBtn.IsEnabled = false;
        }

    }
}
