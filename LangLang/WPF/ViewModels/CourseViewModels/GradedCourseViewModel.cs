
using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    internal class GradedCourseViewModel
    {
        public ObservableCollection<CourseViewModel> GradedCourses {  get; set; }
        public CourseViewModel SelectedCourse {  get; set; }
        private CourseService _service { get; set; }
        public GradedCourseViewModel()
        {
            _service = new();
            SelectedCourse = new();
            GradedCourses = new();
            Update();
        }

        public void StartSmartSystem(bool knowledgePriority)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to run the smart system for the selected course?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            Course course = SelectedCourse.ToCourse();
            List<Student> students = SmartSystem.GetTopStudents(course, knowledgePriority); 
            foreach (Student student in students)
            {
                string subject = $"Congratulations on Your Outstanding Achievement in {course.Language} {course.Level} Course!";
                string body = $"Dear {student.Profile.Name},\r\n\r\nI am delighted to extend my heartfelt " +
                                "congratulations to you for being one of the top students in our course. " +
                                "Your dedication, hard work, and outstanding performance have truly set you apart. " +
                                "We are proud to have you as a part of our learning community.\r\n\r\nBest regards,";
                EmailService.SendEmail(student.Profile.Email, subject, body);
            }

            course.GratitudeEmailSent = true;
            _service.Update(course);
            Update();
        }

        public void Update()
        {
            GradedCourses.Clear();
            foreach (Course course in _service.GetGraded())
            {
                GradedCourses.Add(new CourseViewModel(course)); 
            }
        }
       
    }
}
