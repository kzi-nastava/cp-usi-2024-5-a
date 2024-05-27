
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
            var senderService = new SenderService();
            senderService.SendGratitudeMail(course, students);

            course.GratitudeEmailSent = true;
            _service.Update(course);
            ShowSuccess();
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

        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed!");
        }

    }
}
