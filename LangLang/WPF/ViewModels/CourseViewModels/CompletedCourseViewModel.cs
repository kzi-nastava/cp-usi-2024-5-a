using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.TutorViewModels;
using LangLang.WPF.Views.StudentView.AdditionalWindows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CompletedCourseViewModel
    {
        public ObservableCollection<CourseViewModel> Courses { get; set; }
        private List<Course> completedCourses { get; set; }
        public CourseViewModel SelectedCourse { get; set; }
        private Student currentlyLoggedIn { get; set; }
        public CompletedCourseViewModel(Student currentlyLoggedIn)
        {
            Courses = new();
            this.currentlyLoggedIn = currentlyLoggedIn;
        }

        public void SetDataForReview()
        {
            var courseService = new CourseService();
            completedCourses = courseService.GetCompleted(currentlyLoggedIn);

            Courses.Clear();
            foreach (var course in completedCourses)
                Courses.Add(new CourseViewModel(course));
        }

        public bool ToEnableButton()
        {
            return SelectedCourse == null;
        }

        public void TryRateTutor()
        {
            var tutorRatingService = new TutorRatingService();
            if (tutorRatingService.IsRated(currentlyLoggedIn.Id, SelectedCourse.Id) != -1)
            {
                MessageBox.Show("You have already rated this tutor.", "Rating Already Submitted");
                return;
            }
            TutorRatingViewModel tutorRatingDTO = new()
            {
                TutorId = SelectedCourse.TutorId,
                StudentId = currentlyLoggedIn.Id
            };
            TutorRatingWindow ratingWindow = new(tutorRatingDTO, SelectedCourse.TutorFullName);
            ratingWindow.Show();
        }
    }
}
