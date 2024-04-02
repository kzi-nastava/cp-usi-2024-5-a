using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
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
        private CourseController courseController { get; set; }
        private AppController appController;
        private ObservableCollection<CourseDTO> courses;
        private List<Course> coursesForReview;
        private int tutorId { get; set; }
        public CourseSearchWindow(CourseController courseController, int tutorId)
        {

            InitializeComponent();
            DataContext = this;

            this.tutorId = tutorId;
            this.courseController = new CourseController();
            this.courseController = courseController;

            this.appController = appController;

            this.courses = new ObservableCollection<CourseDTO>();
            
            coursesForReview = this.courseController.GetCoursesByTutor(tutorId).Values.ToList<Course>();

            levelCoursecb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));

            //this.courseController.Subscribe((Core.Observer.IObserver)this);
            Update();
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public ObservableCollection<CourseDTO> Courses
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
            coursesForReview =  this.courseController.SearchCourses(this.courseController.GetCoursesByTutor(tutorId).Values.ToList<Course>(), language, level, courseStartDate, duration, !onlinecb.IsChecked);
            Update();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            coursesForReview = this.courseController.GetCoursesByTutor(tutorId).Values.ToList<Course>();
            levelCoursecb.SelectedItem = null;
            Update();
        }
        public void Update()
        {
            Courses.Clear();
            foreach (Course course in coursesForReview)
            {
                Courses.Add(new CourseDTO(course));
            }
        }
    }
}
