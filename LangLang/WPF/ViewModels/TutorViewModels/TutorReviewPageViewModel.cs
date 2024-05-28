
using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class TutorReviewPageViewModel
    {
        public ObservableCollection<TutorViewModel> Tutors { get; set; }
        public List<Tutor> TutorsForReview { get; set; }
        public TutorViewModel SelectedTutor { get; set; }
        public TutorViewModel SearchTutor { get; set; }

        public TutorReviewPageViewModel()
        {
            Tutors = new();
            TutorsForReview = new();
            SetDataForReview();
            Update();
        }

        public void Update(){
            Tutors.Clear();

            foreach (Tutor tutor in TutorsForReview)
                Tutors.Add(new TutorViewModel(tutor));
        }

        public void SetDataForReview()
        {
            var tutorService = new TutorService();
            TutorsForReview = tutorService.GetActive();
        }

        public void DeleteTutor()
        {
            var courseService = new CourseService();
            var tutorService = new TutorService();
            var examService = new ExamSlotService();

            courseService.DeleteByTutor(SelectedTutor.ToTutor());
            examService.DeleteByTutor(SelectedTutor.ToTutor());
            tutorService.Deactivate(SelectedTutor.Id);

            ShowSuccess();
            SetDataForReview();
            Update();
        }

        public void ShowSuccess()
        {
            MessageBox.Show("Tutor is successfully deleted");

        }
        public void Search(DateTime employmentDate, string language, LanguageLevel? level)
        {
            var tutorService = new TutorService();
            TutorsForReview = tutorService.Search(employmentDate, language, level);
            Update();
        }

    }
}
