using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Model;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangLang.View.CourseGUI
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
            LanguageLevel? level = null;
            if (levelCoursecb.SelectedValue != null)
                level = (LanguageLevel)levelCoursecb.SelectedValue;
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
