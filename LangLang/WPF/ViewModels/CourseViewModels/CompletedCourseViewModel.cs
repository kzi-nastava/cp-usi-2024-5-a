using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.DTO;
using LangLang.WPF.Views.StudentView.AdditionalWindows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModel
{
    public class CompletedCourseViewModel
    {
        public ObservableCollection<CourseDTO> Courses { get; set; }
        private List<Course> completedCourses { get; set; }
        public CourseDTO SelectedCourse { get; set; }
        private Student currentlyLoggedIn { get; set; }
        public CompletedCourseViewModel(Student currentlyLoggedIn)
        {
            Courses = new();
            this.currentlyLoggedIn = currentlyLoggedIn;
        }

        public void SetDataForReview()
        {
            var courseService = new CourseController();
            completedCourses = courseService.GetCompleted(currentlyLoggedIn);

            Courses.Clear();
            foreach (var course in completedCourses)
                Courses.Add(new CourseDTO(course));
        }

        public bool ToEnableButton()
        {
            return SelectedCourse == null;
        }

        public void TryRateTutor()
        {
            var tutorRatingService = new TutorRatingService();
            if (tutorRatingService.IsRated(currentlyLoggedIn.Id, SelectedCourse.TutorId))
            {
                MessageBox.Show("You have already rated this tutor.", "Rating Already Submitted");
                return;
            }
            TutorRatingDTO tutorRatingDTO = new()
            {
                TutorId = SelectedCourse.TutorId,
                StudentId = currentlyLoggedIn.Id
            };
            TutorRatingWindow ratingWindow = new(tutorRatingDTO, SelectedCourse.TutorFullName);
            ratingWindow.Show();
        }
    }
}
