using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.WPF.Views.TutorView.Tabs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CoursesDirectorVM
    {
        public ObservableCollection<CourseViewModel> Courses { get; set; }
        public CourseViewModel SelectedCourse { get; set; }

        public CoursesDirectorVM()
        {
            Courses = new();
            SelectedCourse = new();
            SelectedCourse.TutorId = Constants.DELETED_TUTOR_ID;
        }
        public void AssignTutor(CourseViewModel course)
        {
            course.TutorId = SmartSystem.MostSuitableTutor(SelectedCourse.ToCourse());
            if (course.TutorId == -1)
            {
                MessageBox.Show("There is no suitable tutor for this course at the moment.");
                return;
            }
            CourseService service = new();
            service.Update(course.ToCourse());
            MessageBox.Show("Tutor is successfuly assigned.");
            Update();
        }
        public void Update()
        {
            SelectedCourse.TutorId = Constants.DELETED_TUTOR_ID;
            Courses.Clear();
            CourseService courseService = new();
            foreach (Course course in courseService.GetAll())
            {
                Courses.Add(new CourseViewModel(course));
            }
        }
        public bool HasTutor()
        {
            if (SelectedCourse == null) return false;

            if (SelectedCourse.TutorId != Constants.DELETED_TUTOR_ID)
            {
                return true;
            }
            return false;
        }
    }
}
