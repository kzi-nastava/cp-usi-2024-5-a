using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.WPF.ViewModels.TutorViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CreateCourseDirectorVM
    {
        public CourseViewModel Course { get; set; }

        public CreateCourseDirectorVM()
        {
            Course = new CourseViewModel();
        }

        public bool CreatedCourse()
        {
            if (!Course.IsValid)
            {
                MessageBox.Show("Something went wrong. Please check all fields in the form.");
                return false;
            }

            var courseService = new CourseService();
            if (courseService.CanCreateOrUpdate(Course.ToCourse()))
            {
                //Assign(Course);
                courseService.Add(Course.ToCourse());
                MessageBox.Show("Success!");
                return true;
            }
            MessageBox.Show("The course cannot be created, there are time overlaps or no available classroms (if the course is held in a classroom).");
            return false;
        }
        /*
        private void Assign(CourseViewModel course)
        {
            course.TutorId = MostSuitableTutor(Course.ToCourse());
            if(course.TutorId == -1)
            {
                MessageBox.Show("There is no suitable tutor for this course at the moment.");
            }
        }
        */
    }
}
