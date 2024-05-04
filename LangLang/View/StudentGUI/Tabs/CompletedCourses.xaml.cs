using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.StudentGUI.Tabs
{
    public partial class CompletedCourses : UserControl
    {
        private readonly AppController appController;
        private readonly Student currentlyLoggedIn;
        private readonly StudentWindow parentWindow;
        public ObservableCollection<CourseDTO> Courses {  get; set; }
        private List<Course> completedCourses {  get; set; }
        public CourseDTO SelectedCourse { get; set; }
        public CompletedCourses(AppController appController, Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            DataContext = this;
            this.appController = appController;
            this.currentlyLoggedIn = currentlyLoggedIn;
            this.parentWindow = parentWindow;
            Courses = new();
            SetDataForReview();
            rateTutorBtn.IsEnabled = false;
        }

        private void SetDataForReview()
        {
            var courseController = appController.CourseController;
            completedCourses = courseController.GetCompletedCourses(currentlyLoggedIn.Id, appController);

            foreach (var course in completedCourses)
                Courses.Add(new CourseDTO(course, appController));
        }

        private void CompletedCoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCourse == null) rateTutorBtn.IsEnabled = false;
            else rateTutorBtn.IsEnabled = true;
        }


        private void rateTutorBtn_Click(object sender, RoutedEventArgs e)
        {
            if (appController.TutorRatingController.IsRated(currentlyLoggedIn.Id, SelectedCourse.Id))
            {
                MessageBox.Show("You have already rated this tutor.", "Rating Already Submitted");
                return;
            }
            TutorRatingDTO tutorRatingDTO = new()
            {
                TutorId = SelectedCourse.TutorId,
                StudentId = currentlyLoggedIn.Id
            };
            TutorRating ratingWindow = new(appController, tutorRatingDTO, SelectedCourse.TutorFullName);
            ratingWindow.Show();
        }
    }
}
