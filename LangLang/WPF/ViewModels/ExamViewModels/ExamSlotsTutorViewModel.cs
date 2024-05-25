using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Core;
using LangLang.Domain.Models;
using LangLang.View.ExamSlotGUI;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.ViewModels.RequestsViewModels;
using LangLang.WPF.Views.StudentView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
  
        public void UpdateExam()
        {
            ExamSlotService examSlotService = new();
            ExamSlotUpdateWindow updateWindow = new(SelectedExamSlot.Id, LoggedIn);
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
