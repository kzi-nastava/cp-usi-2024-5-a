using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using System.Collections.ObjectModel;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CoursesDirectorVM
    {
        public ObservableCollection<CourseViewModel> Courses { get; set; }

        public CourseViewModel SelectedCourse { get; set; }

        public CoursesDirectorVM()
        {
            Courses = new();
        }

        public void Update()
        {
            Courses.Clear();
            CourseService courseService = new();
            foreach (Course course in courseService.GetAll())
            {
                Courses.Add(new CourseViewModel(course));
            }
        }
    }
}
