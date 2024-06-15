
using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.LanguageLevelViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class TutorReviewPageViewModel
    {
        public ObservableCollection<TutorViewModel> Tutors { get; set; }
        public ObservableCollection<LanguageLevelViewModel> Skills {  get; set; } 
        public List<Tutor> TutorsForReview { get; set; }
        public List<LanguageLevel> skillsForReview {  get; set; }
        public TutorViewModel SelectedTutor { get; set; }
        public TutorViewModel SearchTutor { get; set; }

        public TutorReviewPageViewModel()
        {
            Tutors = new();
            Skills = new();
            TutorsForReview = new();
            skillsForReview = new();
            SetDataForReview();
            Update();
        }

        public void Update()
        {
            Tutors.Clear();
            foreach (Tutor tutor in TutorsForReview)
                Tutors.Add(new TutorViewModel(tutor));
        }

        public void UpdateSkills()
        {
            Skills.Clear();
            foreach (LanguageLevel ll in skillsForReview)
                Skills.Add(new LanguageLevelViewModel(ll));
        }

        public void SetDataForReview()
        {
            var tutorService = new TutorService();
            TutorsForReview = tutorService.GetActive();
        }

        public void SetSkillsForReview()
        {
            var tutorSkillService = new TutorSkillService();
            skillsForReview = tutorSkillService.GetByTutor(SelectedTutor.ToTutor());
            UpdateSkills();
        }

        public void ClearSkills()
        {
            skillsForReview.Clear();
            UpdateSkills();
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
      
        public void Search(DateTime employmentDate)
        {
            var tutorService = new TutorService();
            TutorsForReview = tutorService.Search(employmentDate);
            Update();
        }

    }
}
