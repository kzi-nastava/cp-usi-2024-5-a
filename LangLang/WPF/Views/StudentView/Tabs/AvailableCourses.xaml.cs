using LangLang.Core.Model;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.StudentView.Tabs
{
    public partial class AvailableCourses : UserControl
    {
        private readonly StudentWindow parentWindow;
        public AvailableCoursesViewModel AvailableCoursesVM { get; set; }
        
        public AvailableCourses(Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            AvailableCoursesVM = new(currentlyLoggedIn);
            DataContext = AvailableCoursesVM;
            this.parentWindow = parentWindow;
            
            SetDataForReview();
            levelCoursecb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            AdjustButton();
        }

        public void SetDataForReview()
        {
            AvailableCoursesVM.SetDataForReview();
        }

        private void AdjustButton()
        {
            if (!AvailableCoursesVM.CanRequestEnroll())
                SendRequestBtn.IsEnabled = false;
        }


        private void SearchCourses(object sender, RoutedEventArgs e)
        {
            string? language = languagetb.Text;
            LanguageLevel? level = null;
            if (levelCoursecb.SelectedValue != null)
                level = (LanguageLevel)levelCoursecb.SelectedValue;
            DateTime courseStartDate = courseStartdp.SelectedDate ?? default;
            bool parsed = int.TryParse(durationtb.Text, out int duration);
            if (!parsed) duration = 0;

            AvailableCoursesVM.Search(language, level, courseStartDate, duration, !onlinecb.IsChecked);
            parentWindow.Update();

        }

        private void ClearCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            AvailableCoursesVM.Clear();
            languagetb.Text = "";
            durationtb.Text = "";
            levelCoursecb.SelectedItem = null;
            courseStartdp.SelectedDate = null;
            parentWindow.Update();
        }

        private void SendRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            AvailableCoursesVM.SendRequest();
            parentWindow.EnrollmentRequestsTab.SetDataForReview();
            parentWindow.Update();
        }


        public void Update()
        {
            AvailableCoursesVM.Update();
        }

    }
}
