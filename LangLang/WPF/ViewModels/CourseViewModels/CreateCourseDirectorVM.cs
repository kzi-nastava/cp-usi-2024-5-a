using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using System.Diagnostics;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CreateCourseDirectorVM
    {
        public CourseViewModel Course { get; set; }

        public CreateCourseDirectorVM()
        {
            Course = new CourseViewModel();
            Course.TutorId = Constants.DELETED_TUTOR_ID;
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
                Trace.WriteLine("Usao da moze da create za " + Course.ToCourse().TutorId);
                AssignTutor(Course);
                courseService.Add(Course.ToCourse());
                MessageBox.Show("Success!");
                return true;
            }
            MessageBox.Show("The course cannot be created, there are time overlaps or no available classroms (if the course is held in a classroom).");
            return false;
        }
        
        private void AssignTutor(CourseViewModel course)
        {
            course.TutorId = SmartSystem.MostSuitableTutor(Course.ToCourse());
            if(course.TutorId == -1)
            {
                MessageBox.Show("There is no suitable tutor for this course at the moment.");
            }
        }
        
    }
}
