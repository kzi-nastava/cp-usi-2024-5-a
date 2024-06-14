using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.Views.StudentView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class AvailableExamsVM
    {
        public readonly Student loggedIn;
        public List<ExamSlot> ExamsForReview { get; set; }
        public ObservableCollection<ExamSlotViewModel> ExamSlots { get; set; }
        private ExamApplication Application { get; set; }
        private StudentWindow _parent;
        public ExamSlotViewModel SelectedExam { get; set; }

        public AvailableExamsVM(Student loggedIn, StudentWindow parent)
        {
            
            this.loggedIn = loggedIn;
            this._parent = parent;
            ExamSlots = new();
            ExamsForReview = new();
            SetDataForReview();

        }

        public void Update()
        {
            ExamSlots.Clear();
            
            foreach(ExamSlot exam in ExamsForReview)
            {
                ExamSlots.Add(new ExamSlotViewModel(exam));
            }
        }
        public void SetDataForReview()
        {
            ExamSlotService examsService = new();
            ExamsForReview = examsService.GetAvailableExams(loggedIn);
        }
        public void SendApplication()
        {
            var studentService = new StudentService();
            bool canApplyForExams = studentService.CanApplyForExams(loggedIn);
            if (canApplyForExams)
            {
                if (SelectedExam == null) return;
                Application = new();
                Application.ExamSlotId = SelectedExam.ToExamSlot().Id;
                Application.StudentId = loggedIn.Id;
                Application.SentAt = DateTime.Now;
                ExamApplicationService appsService = new();
                appsService.Add(Application);

                MessageBox.Show("You successfully applied for exam.");

                SetDataForReview();
                _parent.Update();

            }
            else
            {
                MessageBox.Show("Can't apply for the exam as all results have not yet been received.");
            }

        }
        public void SearchExams(Student loggedIn, DateTime examDate, string language, Level? level)
        {
            ExamSlotService examsService = new();
            ExamsForReview = examsService.SearchByStudent(loggedIn, examDate, language, level);
            Update();
        }


        public void ClearExams()
        {
            SetDataForReview();
            Update();
        }
    }
}
