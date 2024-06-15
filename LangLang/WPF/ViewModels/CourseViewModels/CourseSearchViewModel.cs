using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CourseSearchViewModel
    {
        public ObservableCollection<CourseViewModel> Courses { get; set; }
        private List<Course> CoursesForReview;
        private Tutor LoggedIn { get; set; }
        public CourseSearchViewModel(Tutor tutor)
        {
            LoggedIn = tutor;
            Courses = new ObservableCollection<CourseViewModel>();
            CoursesForReview = new();
            Reset();
            Update();
        }

        public void Reset()
        {
            CourseService courseService = new();
            CoursesForReview = courseService.GetByTutor(LoggedIn.Id);
        }
        public void Update()
        {
            Courses.Clear();

            foreach(Course course in CoursesForReview)
            {
                Courses.Add(new CourseViewModel(course));
            }
        }

        public void Search(String language, Level? level, DateTime courseStartDate, int duration, bool? online)
        {
            CourseService courseService = new();
            CoursesForReview = courseService.SearchCoursesByTutor(LoggedIn.Id, language, level, courseStartDate, duration, !online);
            Update();
        }
    }
}
