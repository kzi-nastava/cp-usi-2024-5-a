using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.ExamViewModels;
using LangLang.WPF.Views.TutorView.Tabs;
using System;
using System.Windows;

namespace LangLang.WPF.Views.TutorView.AddidtionalWindows.CourseView
{
    /// <summary>
    /// Interaction logic for CourseUpdateWindow.xaml
    /// </summary>
    public partial class CourseUpdateWindow : Window
    {
        public CourseUpdateViewModel CourseUpdateVM { get; set; }
        private Courses _parent;
        public CourseUpdateWindow(Courses parent, Course course)
        {
            InitializeComponent();
            _parent = parent;
            CourseUpdateVM = new(course);
            DataContext = CourseUpdateVM;

            languageLvlCb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        private void CourseUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CourseUpdateVM.UpdatedCourse())
            {
                _parent.Update();
                Close();
            }
        }

        // Method enables textbox for maxNumOfStudents when the course is to be held in a classroom

        private void ClasssroomCb_Checked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = true;
            inClassroomErrorTb.Text = "Please enter the maximal number of students, as it is required";
        }

        private void ClasssroomCb_Unchecked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = false;
            inClassroomErrorTb.Text = "";
        }
    }
}
