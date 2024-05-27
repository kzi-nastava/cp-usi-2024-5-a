using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CourseUpdateViewModel
    {
        public CourseViewModel Course { get; set; }
        public CourseUpdateViewModel(Course course)
        {
            Course = new(course);
        }
        public bool UpdatedCourse()
        {
            if (!Course.IsValid)
            {
                MessageBox.Show("Something went wrong. Please check all fields in the form.");
                return false;
            }
            var courseService = new CourseService();
            if (!courseService.CanCreateOrUpdate(Course.ToCourse()))
            {
                MessageBox.Show("The course cannot be updated, there are time overlaps or no available classroms (if the course is held in a classroom).");
                return false;
            }
            courseService.Update(Course.ToCourse());
            MessageBox.Show("Success!");
            return true;
        }
    }
}
