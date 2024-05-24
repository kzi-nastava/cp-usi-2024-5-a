using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.Domain.Models.Enums;
using LangLang.WPF.ViewModels.CourseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for CourseSearchWindow.xaml
    /// </summary>
    public partial class CourseSearchWindow : Window
    {
        private CourseService courseService;
        private ObservableCollection<CourseViewModel> courses;
        private List<Course> coursesForReview;
        private int tutorId { get; set; }
        public CourseSearchWindow(AppController appController, Tutor loggedIn)
        {

            InitializeComponent();
            DataContext = this;

            this.tutorId = tutorId;
            this.courseService = new();

            this.courses = new ObservableCollection<CourseViewModel>();
            
            coursesForReview = this.courseService.GetByTutor(tutorId);

            levelCoursecb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));

            //this.courseController.Subscribe((Core.Observer.IObserver)this);
            Update();
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public ObservableCollection<CourseViewModel> Courses
        {
            get { return courses; }
            set { courses = value; }
        }
        private void SearchCourses(object sender, RoutedEventArgs e)
        {
            string? language = languagetb.Text;
            LanguageLevel? level = null;
            if (levelCoursecb.SelectedValue != null)
                level = (LanguageLevel)levelCoursecb.SelectedValue;
            DateTime courseStartDate = courseStartdp.SelectedDate ?? default;
            int duration = 0;
            int.TryParse(durationtb.Text, out duration);
            coursesForReview =  this.courseService.SearchCoursesByTutor(tutorId, language, level, courseStartDate, duration, !onlinecb.IsChecked);
            Update();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            coursesForReview = this.courseService.GetByTutor(tutorId);
            levelCoursecb.SelectedItem = null;
            Update();
        }
        public void Update()
        {
            Courses.Clear();
            foreach (Course course in coursesForReview)
            {
                Courses.Add(new CourseViewModel(course));
            }
        }
    }
}
