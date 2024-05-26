using LangLang.Domain.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LangLang.BusinessLogic.UseCases;
using LangLang.WPF.ViewModels.StudentViewModels;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.RequestsViewModels;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for DurationOfCourseWindow.xaml
    /// </summary>
    public partial class DurationOfCourseWindow : Window
    {
        public DuringCourseViewModel DuringCourseViewModel { get; set; }
        public DurationOfCourseWindow(CourseViewModel course)
        {
            InitializeComponent();
            DuringCourseViewModel = new(course);
            DataContext = DuringCourseViewModel;

            DisableStudentsForm();
            DisableWithdrawalForm();
            DuringCourseViewModel.Update();
        }
        
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnableStudnetsForm()
        {
            penaltyPointBtn.IsEnabled = true;
        }
        private void DisableStudentsForm()
        {
            penaltyPointBtn.IsEnabled = false;
        }
        private void EnableWithdrawalForm()
        {
            acceptBtn.IsEnabled = true;
            rejectBtn.IsEnabled = true;
        }
        private void DisableWithdrawalForm()
        {
            acceptBtn.IsEnabled = false;
            rejectBtn.IsEnabled = false;
        }
        private void StudentTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DuringCourseViewModel.SelectedStudent != null)
                EnableStudnetsForm();
            else
                DisableStudentsForm();
        }
        private void WithdrawalTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DuringCourseViewModel.SelectedWithdrawal != null)
                EnableWithdrawalForm();
            else
                DisableWithdrawalForm();
        }

        private void PenaltyBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to give the student a penalty point?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DisableStudentsForm();
                DuringCourseViewModel.GivePenaltyPoint();
                ShowSuccess();
            }
        }
        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to accept the withdrawal?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DisableWithdrawalForm();
                DuringCourseViewModel.AcceptWithdrawal();
                ShowSuccess();
            }
        }
        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to reject the withdrawal?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DisableWithdrawalForm();
                DuringCourseViewModel.RejectWithdrawal();
                ShowSuccess();
            }
        }
    }
}
