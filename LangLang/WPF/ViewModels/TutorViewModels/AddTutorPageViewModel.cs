using LangLang.Domain.Enums;
using System.Windows;
using System;
using LangLang.BusinessLogic.UseCases;
using System.Collections.ObjectModel;
using LangLang.WPF.ViewModels.LanguageLevelViewModels;
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class AddTutorPageViewModel
    {
        public ObservableCollection<LanguageLevelViewModel> AddedSkills {  get; set; }
        public List<LanguageLevel> skills {  get; set; }
        public LanguageLevelViewModel NewSkill {  get; set; }
        public TutorViewModel Tutor { get; set; }
        private int tutorId {  get; set; }

        public AddTutorPageViewModel() {
            AddedSkills = new();
            Tutor = new TutorViewModel();
            NewSkill = new LanguageLevelViewModel();
            SetSkills();
            Update();
        }

        public void SetSkills()
        {
            TutorSkillService tutorSkillService = new();
            Tutor.Id = tutorId;
            skills = tutorSkillService.GetByTutor(Tutor.ToTutor());
        }

        public void Update()
        {
            AddedSkills.Clear();
            foreach (LanguageLevel skill in skills)
            {
                AddedSkills.Add(new LanguageLevelViewModel(skill));
            }
        }

        public bool AddTutor() {
            
            PopulateDefaults();

            var profileService = new ProfileService();
            var tutorService = new TutorService();

            if (!Tutor.IsValid)
                MessageBox.Show("Fields are not valid.");

            else if (profileService.EmailExists(Tutor.Email)) 
                MessageBox.Show("Email already exists. Try with a different email address.");

            else
            {
                tutorId = tutorService.Add(Tutor.ToTutor());
                MessageBox.Show("Success!");
                return true;
            }
            return false;

        }

        private void PopulateDefaults()
        {
            Tutor.EmploymentDate = DateTime.Now;
            Tutor.Role = UserType.Tutor;
        }

        public void AddSkill()
        {
            var tutorSkillService = new TutorSkillService();
            var languageLevelService = new LanguageLevelService();
            int skillId = languageLevelService.Add(NewSkill.ToLanguageLevel());

            if (tutorSkillService.AlreadyAdded(tutorId, skillId)) return;

            var newSkill = new TutorSkill(tutorId, skillId);
            tutorSkillService.Add(newSkill);
            SetSkills();
            Update();

            MessageBox.Show("Success!");
        }

    }
}
