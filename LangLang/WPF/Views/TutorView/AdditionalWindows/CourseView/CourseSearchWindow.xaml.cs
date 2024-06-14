using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.TutorView.AdditionalWindows.CourseView
{
    /// <summary>
    /// Interaction logic for CourseSearchWindow.xaml
    /// </summary>
    public partial class CourseSearchWindow : Window
    {
        public CourseSearchViewModel CourseSearchViewModel { get; set; }
        public CourseSearchWindow(Tutor loggedIn)
        {
            InitializeComponent();
            CourseSearchViewModel = new(loggedIn);
            DataContext = CourseSearchViewModel;

            levelCoursecb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void SearchCourses(object sender, RoutedEventArgs e)
        {
            string? language = languagetb.Text;
            Level? level = null;
            if (levelCoursecb.SelectedValue != null)
                level = (Level)levelCoursecb.SelectedValue;
            DateTime courseStartDate = courseStartdp.SelectedDate ?? default;
            int duration = 0;
            int.TryParse(durationtb.Text, out duration);
            CourseSearchViewModel.Search(language, level, courseStartDate, duration, onlinecb.IsChecked);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            levelCoursecb.SelectedItem = null;
            CourseSearchViewModel.Reset();
            CourseSearchViewModel.Update();
        }
    }
}
