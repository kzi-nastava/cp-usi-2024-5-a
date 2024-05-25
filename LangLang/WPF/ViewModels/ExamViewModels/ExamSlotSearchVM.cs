﻿using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamSlotSearchVM
    {
        public ObservableCollection<ExamSlotViewModel> ExamSlots { get; set; }
        private List<ExamSlot> examSlotsForReview;
        public Tutor loggedIn;

        public ExamSlotSearchVM(Tutor loggedIn)
        {
            ExamSlots = new ObservableCollection<ExamSlotViewModel>();
            examSlotsForReview = new List<ExamSlot>();
            this.loggedIn = loggedIn;
            ExamSlotService examsService = new();
            examSlotsForReview = examsService.GetExams(loggedIn);

            foreach (ExamSlot exam in examSlotsForReview)
            {
                ExamSlots.Add(new ExamSlotViewModel(exam));
            }


        }
        public void SetDataForView()
        {

            ExamSlots.Clear();
            foreach (ExamSlot exam in examSlotsForReview)
            {
                ExamSlots.Add(new ExamSlotViewModel(exam));
            }
        }
        public void SearchExams(Tutor loggedIn,DateTime examDate,string language,LanguageLevel? level)
        {
            ExamSlotService examsService = new();
            examSlotsForReview = examsService.SearchByTutor(loggedIn, examDate, language, level);
            SetDataForView();
        }
        

        public void ClearExams()
        {
            ExamSlotService examsService = new();
            examSlotsForReview = examsService.GetExams(loggedIn);
            SetDataForView();
        }
    }
}
