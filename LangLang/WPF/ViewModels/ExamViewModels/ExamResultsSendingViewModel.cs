using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamResultsSendingViewModel
    {

        public ObservableCollection<ExamSlotViewModel> ExamSlots { get; set; }
        public ExamSlotViewModel SelectedExam { get; set; }

        public ExamResultsSendingViewModel() {
            ExamSlots = new();
            SelectedExam = new();
        }

        public void Update()
        {
            var examService = new ExamSlotService();

            ExamSlots.Clear();

            foreach (ExamSlot exam in examService.GetGraded())
                ExamSlots.Add(new ExamSlotViewModel(exam));

        }

        public void SendResults()
        {
            var senderService = new SenderService();
            senderService.SendResults(SelectedExam.ToExamSlot());

            ShowSuccess();

            UpdateExam();
            Update();
        }

        private void UpdateExam()
        {
            var examService = new ExamSlotService();
            SelectedExam.ExamineesNotified = true;
            examService.Update(SelectedExam.ToExamSlot());
        }
     
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed!");
        }

    }

}
