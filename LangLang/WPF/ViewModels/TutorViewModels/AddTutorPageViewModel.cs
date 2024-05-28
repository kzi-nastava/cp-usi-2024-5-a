using LangLang.Domain.Enums;
using System.Windows;
using System;
using LangLang.BusinessLogic.UseCases;

namespace LangLang.WPF.ViewModels.TutorViewModels
{

    public class AddTutorPageViewModel
    {
        public TutorViewModel Tutor { get; set; }

        public AddTutorPageViewModel() {
            Tutor = new TutorViewModel();
        }

        public void AddTutor() {
            
            PopulateDefaults();

            var profileService = new ProfileService();
            var tutorService = new TutorService();

            if (!Tutor.IsValid)
                MessageBox.Show("Fields are not valid.");

            else if (profileService.EmailExists(Tutor.Email)) 
                MessageBox.Show("Email already exists. Try with a different email address.");

            else
            {
                tutorService.Add(Tutor.ToTutor());
                MessageBox.Show("Success!");
            }

        }

        private void PopulateDefaults()
        {
            Tutor.EmploymentDate = DateTime.Now;
            Tutor.Role = UserType.Tutor;
        }

        public void AddSkill(string language, LanguageLevel level)
        {
            Tutor.Language.Add(language);
            Tutor.Level.Add(level);
            MessageBox.Show("Success!");
        }

    }
}
