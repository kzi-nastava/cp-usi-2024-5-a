using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CoursesTutorViewModel
    {
        public ObservableCollection<CourseViewModel> Courses { get; set; }
        private List<Course> courses;

        public CourseViewModel SelectedCourse { get; set; }
        public Tutor LoggedIn { get; set; }

        public CoursesTutorViewModel(Tutor loggedIn)
        {
            Courses = new();
            LoggedIn = loggedIn;
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
    }
}
