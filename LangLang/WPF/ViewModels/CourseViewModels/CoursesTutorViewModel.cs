using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.WPF.Views.TutorView.AdditionalWindows.CourseView;
using LangLang.WPF.Views.TutorView.Tabs;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CoursesTutorViewModel
    {
        public ObservableCollection<CourseViewModel> Courses { get; set; }

        public CourseViewModel SelectedCourse { get; set; }
        public Tutor LoggedIn { get; set; }
        private readonly CourseService service;
        private readonly Courses parent;

        public CoursesTutorViewModel(Tutor loggedIn, Courses parent)
        {
            Courses = new();
            LoggedIn = loggedIn;
            service = new();
            this.parent = parent;
        }

        public void Update()
        {
            Courses.Clear();
            CourseService courseService = new();
            foreach (Course course in courseService.GetByTutor(LoggedIn.Id))
            {
                Courses.Add(new CourseViewModel(course));
            }
        }

        public void EnterGrade()
        {
            var course = SelectedCourse.ToCourse();
            if (!(service.GetEnd(course) < DateTime.Now))
            {
                MessageBox.Show("The course is not finished.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            EnterGradesWindow gradesWindow = new(SelectedCourse);
            gradesWindow.Show();
        }

        public void DurationOfCurse()
        {
            var course = SelectedCourse.ToCourse();

            if (!service.IsActive(course))
            {
                MessageBox.Show("The course is not active.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DurationOfCourseWindow courseWindow = new(SelectedCourse);
            courseWindow.Show(); 
        }

        public void CourseEnrollments()
        {
            if (!SelectedCourse.Modifiable)
            {
                MessageBox.Show("The enrollments for this course have already been confirmed. No further changes are allowed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            CourseEnrollmentsWindow enrollmentsWindow = new(parent, SelectedCourse);
            enrollmentsWindow.Show();
        }

        public void UpdateCourse()
        {
            if (!SelectedCourse.ToCourse().CanChange())
            {
                MessageBox.Show($"Selected course cannot be updated, it has already started or there are less than {Constants.COURSE_MODIFY_PERIOD} days before course start.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            CourseUpdateWindow updateWindow = new(parent, SelectedCourse.ToCourse());
            updateWindow.Show();
        }

        public void DeleteCourse()
        {
            if (!SelectedCourse.ToCourse().CanChange())
            {
                MessageBox.Show($"Selected course cannot be deleted, it has already started or there are less than {Constants.COURSE_MODIFY_PERIOD} days before course start.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            CourseService courseService = new();
            courseService.Delete(SelectedCourse.ToCourse());
            Update();
            MessageBox.Show("The course has been successfully deleted.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
