﻿using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace LangLang.View.ExamSlotGUI
{
    /// <summary>
    /// Interaction logic for EnterResults.xaml
    /// </summary>
    public partial class EnterResults : Window
    {
        public ExamResultViewModel SelectedResult { get; set; }
        public ObservableCollection<ExamResultViewModel> ExamResults { get; set; }

        private ExamSlotViewModel exam;

        public EnterResults(ExamSlotViewModel selectedExam)
        {
            InitializeComponent();
            DataContext = this;

            exam = selectedExam;
            ExamResults = new ();
            SelectedResult = new();

            DisableForm();
            Update();
        }

        private void Update()
        {
            ExamResults.Clear();

            var resultService = new ExamResultService();

            if (!exam.ResultsGenerated)
            {
                RefreshExam();
                GenerateResults();
            }

            foreach (ExamResult exam in resultService.GetByExam(exam.ToExamSlot()))
            {
                ExamResults.Add(new ExamResultViewModel(exam));
            }

        }

        private void DisableForm()
        {
            nameTB.IsEnabled = false;
            lastnameTB.IsEnabled = false;
            emailTB.IsEnabled = false;
            readingPointsTB.IsEnabled = false;
            listeningPointsTB.IsEnabled = false;
            writingPointsTB.IsEnabled = false;
            speakingPointsTB.IsEnabled = false;
            confirmResultBtn.IsEnabled = false;
        }

        private void EnableForm()
        {
            readingPointsTB.IsEnabled = true;
            listeningPointsTB.IsEnabled = true;
            writingPointsTB.IsEnabled = true;
            speakingPointsTB.IsEnabled = true;
            confirmResultBtn.IsEnabled = true;
        }

        private void ExamResultDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedResult != null)
            {
                FillForm();
                EnableForm();

            } else {
                DisableForm();
                ClearForm();
            }
        }

        private void FillForm()
        {
            nameTB.Text = SelectedResult.Name;
            lastnameTB.Text = SelectedResult.LastName;
            emailTB.Text = SelectedResult.Email;
            readingPointsTB.Text = SelectedResult.ReadingPoints.ToString();
            listeningPointsTB.Text = SelectedResult.ListeningPoints.ToString();
            writingPointsTB.Text = SelectedResult.WritingPoints.ToString();
            speakingPointsTB.Text = SelectedResult.SpeakingPoints.ToString();
        }

        private void ClearForm()
        {
            nameTB.Text = string.Empty;
            lastnameTB.Text = string.Empty;
            emailTB.Text = string.Empty;
            readingPointsTB.Text = string.Empty;
            listeningPointsTB.Text = string.Empty;
            writingPointsTB.Text = string.Empty;
            speakingPointsTB.Text = string.Empty;
        }

        private void confirmResultBtn_Click(object sender, RoutedEventArgs e)
        {
            var resultService = new ExamResultService();

            if (SelectedResult.Outcome == ExamOutcome.NotGraded && SelectedResult.IsValid)
            {
                UpdateDTO();
                resultService.Update(SelectedResult.ToExamResult());
                ShowSuccess();
                Update();
            }
            else
            {
                MessageBox.Show("The change can only be executed if the student is not graded and if the input is valid.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            ClearForm();
        }

        private void UpdateDTO()
        {
                SelectedResult.ListeningPoints = listeningPointsTB.Text;
                SelectedResult.WritingPoints = writingPointsTB.Text;
                SelectedResult.SpeakingPoints = speakingPointsTB.Text;
                SelectedResult.ReadingPoints = readingPointsTB.Text;
        }

        private void RefreshExam()
        {
            var examSlotService = new ExamSlotService();

            exam.ResultsGenerated = true;
            examSlotService.Update(exam.ToExamSlot());
        }

        private void GenerateResults()
        {

            var resultService = new ExamResultService();
            var applicationService = new ExamApplicationService();

            List<ExamApplication> applications = applicationService.GetApplications(exam.Id);
            foreach (ExamApplication application in applications)  // for each application for exam, default result is generated
                resultService.Add(application.StudentId, exam.Id);
        }

        private void ShowSuccess() // TODO: move to utils
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
