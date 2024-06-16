using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class EnterResultsViewModel
    {
        public ExamResultViewModel SelectedResult { get; set; }
        public ObservableCollection<ExamResultViewModel> ExamResults { get; set; }
        public ExamSlotViewModel Exam { get; set; }

        public EnterResultsViewModel(ExamSlotViewModel exam)
        {
            SelectedResult = new();
            ExamResults = new();
            Exam = exam;
            Update();
        }

        public void Update()
        {
            ExamResults.Clear();

            var resultService = new ExamResultService();

            if (!Exam.ResultsGenerated)
            {
                    RefreshExam();
                    GenerateResults();
            }

            foreach (ExamResult exam in resultService.GetByExam(Exam.ToExamSlot()))
            {
                ExamResults.Add(new ExamResultViewModel(exam));
            }

        }

        public void ConfirmResult()
        {
            var resultService = new ExamResultService();

            if (SelectedResult.Outcome == ExamOutcome.NotGraded && SelectedResult.IsValid)
            {
                resultService.Update(SelectedResult.ToExamResult());
                ShowSuccess();
                Update();
            }
            else
                MessageBox.Show("The change can only be executed if the student is not graded and if the input is valid.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshExam()
        {
            var examSlotService = new ExamSlotService();
            Exam.ResultsGenerated = true;
            examSlotService.Update(Exam.ToExamSlot());
        }

        private void GenerateResults()
        {
            var resultService = new ExamResultService();
            var applicationService = new ExamApplicationService();

            List<ExamApplication> applications = applicationService.GetApplications(Exam.Id);
            foreach (ExamApplication application in applications)  // for each application for exam, default result is generated
                resultService.Add(application.StudentId, Exam.Id);
        }

    }
}
