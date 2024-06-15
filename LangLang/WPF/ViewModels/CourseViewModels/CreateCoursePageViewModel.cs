using LangLang.BusinessLogic.UseCases;
using LangLang.WPF.ViewModels.TutorViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CreateCoursePageViewModel
    {
        public CourseViewModel Course { get; set; }

        public CreateCoursePageViewModel()
        {
            Course = new CourseViewModel();
        }

        public bool CreatedCourse(int tutorId)
        {
            if (!Course.IsValid)
            {
                MessageBox.Show("Something went wrong. Please check all fields in the form.");
                return false;
            }

            Course.CreatedAt = DateTime.Now;
            Course.TutorId = tutorId;

            var courseService = new CourseService();
            if (courseService.CanCreateOrUpdate(Course.ToCourse()))
            {
                courseService.Add(Course.ToCourse());
                MessageBox.Show("Success!");
                return true;
            }
            MessageBox.Show("The course cannot be created, there are time overlaps or no available classroms (if the course is held in a classroom).");
            return false;
        }
    }
}
