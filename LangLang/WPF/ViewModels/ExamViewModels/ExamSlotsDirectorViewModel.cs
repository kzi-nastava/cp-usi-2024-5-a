using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamSlotsDirectorViewModel
    {
        public ObservableCollection<ExamSlotViewModel> ExamSlots { get; set; }
        public ExamSlotsDirectorViewModel()
        {
            ExamSlots = new ObservableCollection<ExamSlotViewModel>();
            SetDataForReview();
        }
        public void SetDataForReview()
        {
            ExamSlots.Clear();
            ExamSlotService examSlotService = new();

            foreach (ExamSlot exam in examSlotService.GetAll())
            {
                ExamSlots.Add(new ExamSlotViewModel(exam));
            }
        }
    }
}
