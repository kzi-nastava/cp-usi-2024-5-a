using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.WPF.Views.TutorView.Tabs;
using LangLang.WPF.Views.TutorView.AdditionalWindows.ExamSlotView;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamSlotsTutorViewModel
    {
        public ObservableCollection<ExamSlotViewModel> ExamSlots { get; set; }
        public ExamSlotViewModel SelectedExamSlot { get; set; }
        public Tutor LoggedIn { get;set; }
        public ExamSlotsTutorViewModel(Tutor LoggedIn)
        {
            this.LoggedIn = LoggedIn;
            ExamSlots = new ObservableCollection<ExamSlotViewModel>();
            SetDataForReview();
        }
        public void SetDataForReview()
        {
            ExamSlots.Clear();
            ExamSlotService examSlotService = new();

            foreach (ExamSlot exam in examSlotService.GetExams(LoggedIn))
            {
                ExamSlots.Add(new ExamSlotViewModel(exam));
            }
        }

        public void EnterResults()
        {

            if (SelectedExamSlot.ExamDate.AddHours(Constants.EXAM_DURATION) < DateTime.Now) // after the EXAM_DURATION-hour exam concludes, it is possible to open a window.
            {
                EnterResults resultsWindow = new(SelectedExamSlot);
                resultsWindow.Show();
            }
            else
                MessageBox.Show("This window can be opened once the exam is passed!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void SeeApplications()
        {
            var examService = new ExamSlotService();
            if (examService.ApplicationsVisible(SelectedExamSlot.Id) && SelectedExamSlot.Applicants != 0)
            {
                ExamApplications applicationsWindow = new(SelectedExamSlot);
                applicationsWindow.Show();
            }
            else
                MessageBox.Show($"If there are applications, they can only be viewed {Constants.PRE_START_VIEW_PERIOD} days before exam and during the exam.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void UpdateExam(ExamsReview window)
        {
            if (!IsExamSelected())
            {
                MessageBox.Show("Please select exam.");
                return;
            }
            ExamSlotService examSlotService = new();
            ExamSlotUpdateWindow updateWindow = new(SelectedExamSlot.Id, LoggedIn,window);
            if (examSlotService.CanBeUpdated(SelectedExamSlot.ToExamSlot()))
                updateWindow.Show();
            else
                MessageBox.Show($"Can't update exam, there is less than {Constants.EXAM_MODIFY_PERIOD} days before exam or exam has passed.");
        }

        public void DeleteExam()
        {
            ExamSlotService examSlotService = new();
            if (!examSlotService.Delete(SelectedExamSlot.Id))
            {
                MessageBox.Show($"Can't delete exam, there is less than {Constants.EXAM_MODIFY_PERIOD} days before exam.");
            }
            else
            {
                MessageBox.Show("Exam slot successfully deleted.");
            }

            SetDataForReview();
        }
        public bool IsExamSelected()
        {
            return SelectedExamSlot != null;
        }

    }
}
