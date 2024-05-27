
using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.TutorViewModels
{
    public class TutorReviewPageViewModel
    {
        public ObservableCollection<TutorViewModel> Tutors { get; set; }
        private List<Tutor> tutors;
        public TutorViewModel SelectedTutor { get; set; }
        public TutorViewModel SearchTutor { get; set; }

        public TutorReviewPageViewModel()
        {
            Tutors = new();
        }

        public void Update(){
            var tutorService = new TutorService();
            tutors = tutorService.GetActive();

            Tutors.Clear();

            foreach (Tutor tutor in tutors)
                Tutors.Add(new TutorViewModel(tutor));
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
            Update();
        }

        public void ShowSuccess()
        {
            MessageBox.Show("Tutor is successfully deleted");

        }
        public void Search()
        {
            var tutorService = new TutorService();

            // TODO: implement when skill is refactored

            Update();
        }

    }
}
