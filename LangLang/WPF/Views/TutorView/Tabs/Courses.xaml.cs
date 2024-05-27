using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.Views.TutorView.AdditionalWindows.CourseView;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.TutorView.Tabs
{
    /// <summary>
    /// Interaction logic for Courses.xaml
    /// </summary>
    public partial class Courses : UserControl
    {
        public CoursesTutorViewModel CoursesViewModel { get; set; }
        public Courses(Tutor loggedIn)
        {
            InitializeComponent();
            CoursesViewModel = new(loggedIn);
            DataContext = CoursesViewModel;
            DisableButtons();
            CoursesViewModel.Update();
        }

        private void CoursesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CoursesViewModel.SelectedCourse == null)
            {
                DisableButtons();
            }
            else
            {
                EnableButtons();
            }
        }
        private void DisableButtons()
        {
            courseUpdateBtn.IsEnabled = false;
            courseDeleteBtn.IsEnabled = false;
            courseEnrollmentBtn.IsEnabled = false;
            courseEnterGradeBtn.IsEnabled = false;
            courseDurationBtn.IsEnabled = false;
        }

        private void EnableButtons()
        {
            courseUpdateBtn.IsEnabled = true;
            courseDeleteBtn.IsEnabled = true;
            courseEnrollmentBtn.IsEnabled = true;
            courseEnterGradeBtn.IsEnabled = true;
            courseDurationBtn.IsEnabled = true;
        }
        private void CourseCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseCreateWindow createWindow = new(this);
            createWindow.Show();
        }

        private void CourseSearchWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseSearchWindow searchWindow = new(CoursesViewModel.LoggedIn);
            searchWindow.Show();
        }


        private void CourseUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesViewModel.SelectedCourse.ToCourse().CanChange())
            {
                CourseUpdateWindow updateWindow = new(this, CoursesViewModel.SelectedCourse.ToCourse());
                updateWindow.Show();
            }
            else
            {
                MessageBox.Show($"Selected course cannot be updated, it has already started or there are less than {Constants.COURSE_MODIFY_PERIOD} days before course start.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CourseDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesViewModel.SelectedCourse.ToCourse().CanChange())
            {
                CourseService courseService = new();
                courseService.Delete(CoursesViewModel.SelectedCourse.Id);
                CoursesViewModel.Update();
                MessageBox.Show("The course has been successfully deleted.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Selected course cannot be deleted, it has already started or there are less than {Constants.COURSE_MODIFY_PERIOD} days before course start.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EnterGradeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesViewModel.SelectedCourse.ToCourse().GetEnd() < DateTime.Now)
            {
                EnterGradesWindow gradesWindow = new(CoursesViewModel.SelectedCourse);
                gradesWindow.Show();
            }
            else
            {
                MessageBox.Show("The course is not finished.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void DurationOfCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesViewModel.SelectedCourse.ToCourse().IsActive())
            {
                DurationOfCourseWindow courseWindow = new(CoursesViewModel.SelectedCourse);
                courseWindow.Show();
            }
            else
            {
                MessageBox.Show("The course is not active.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void CourseEnrollmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesViewModel.SelectedCourse.Modifiable)
            {
                CourseEnrollmentsWindow enrollmentsWindow = new(this, CoursesViewModel.SelectedCourse);
                enrollmentsWindow.Show();
            }
            else
            {
                MessageBox.Show("The enrollments for this course have already been confirmed. No further changes are allowed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public void Update()
        {
            CoursesViewModel.Update();
        }
    }
}
