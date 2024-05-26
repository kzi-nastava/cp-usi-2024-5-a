using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.RequestsViewModels;
using LangLang.WPF.ViewModels.RequestViewModels;
using LangLang.WPF.Views.TutorView.Tabs;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for CourseEnrollmentsWindow.xaml
    /// </summary>
    public partial class CourseEnrollmentsWindow : Window
    {
        public CourseEnrollmentsPageViewModel CourseEnrollmentsVM { get; set; }
        private Courses _parent;
        private CourseViewModel _course;
        public CourseEnrollmentsWindow(Courses parent, CourseViewModel course)
        {
            InitializeComponent();
            _parent = parent;
            _course = course;
            CourseEnrollmentsVM = new(course);
            DataContext = CourseEnrollmentsVM;

            rejectBtn.IsEnabled = false;
            conifrmListBtn.IsEnabled = true;

            CourseEnrollmentsVM.Update();
        }

        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to reject " + CourseEnrollmentsVM.SelectedEnrollmentRequest.StudentName + " " + CourseEnrollmentsVM.SelectedEnrollmentRequest.StudentLastName + " from the course?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DisableForm();
                CourseEnrollmentsVM.RejectEnrollment();
                ShowSuccess();
            }
            conifrmListBtn.IsEnabled = true;
        }
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AcceptListBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to confirm list?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            DisableForm();
            CourseEnrollmentsVM.AcceptEnrollments();
            _parent.Update();
            ShowSuccess();
            Close();
        }
      
        private void EnrollmentTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CourseEnrollmentsVM.SelectedEnrollmentRequest != null && _course.Modifiable)
                rejectBtn.IsEnabled = true;
            else
                rejectBtn.IsEnabled = false;
        }
        private void DisableForm()
        {
            rejectBtn.IsEnabled = false;
            conifrmListBtn.IsEnabled = false;
        }
    }
}
