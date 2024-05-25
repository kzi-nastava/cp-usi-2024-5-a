using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.View.ExamSlotGUI;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.Views.TutorView.Tabs;
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
