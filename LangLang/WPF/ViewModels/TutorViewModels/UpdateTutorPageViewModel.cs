using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System.Windows;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class UpdateTutorPageViewModel
    {
        public TutorViewModel Tutor { get; set; }

        public UpdateTutorPageViewModel(TutorViewModel tutor)
        {
            Tutor = tutor;
        }
        public void Update()
        {

            var tutorService = new TutorService();
            var ProfileService = new ProfileService();

            Tutor tutor = tutorService.Get(Tutor.Id);
            if (ProfileService.EmailExists(Tutor.Email, tutor.Profile.Id, UserType.Tutor))
                MessageBox.Show(" Email already exists. Try with a different email address.");

            else if (!Tutor.IsValid)
                MessageBox.Show("Data is not valid");

            else
            {
                tutorService.Update(Tutor.ToTutor());
                MessageBox.Show("Successfully updated!");
            }

        }
    }
}
