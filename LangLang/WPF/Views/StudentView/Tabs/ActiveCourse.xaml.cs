using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.StudentView.Tabs
{
    public partial class ActiveCourseTab : UserControl
    {
        public ActiveCourseViewModel ActiveCourseVM {  get; set; }
        public ActiveCourseTab(Student currentlyLoggedIn)
        {
            InitializeComponent();
            ActiveCourseVM = new(currentlyLoggedIn);
            DataContext = ActiveCourseVM;
            FillCourseInfo();
            AdjustButton();
        }

        private void FillCourseInfo()
        {
            if (!ActiveCourseVM.SetCourse())
            {
                HideWithdrawalBtn();
            }
        }

        private void HideWithdrawalBtn()
        {
            untilEndTb.Text = "You are currently not enrolled in any courses. " +
                            "\nYou can request enrollment or wait for the tutor to accept your request.";
            CourseWithdrawalBtn.Visibility = Visibility.Collapsed;
        }
        private void CourseWithdrawalBtn_Click(object sender, RoutedEventArgs e)
        {
            ActiveCourseVM.WithdrawalFromCourse();
        }

        private void AdjustButton()
        {
            if (CourseWithdrawalBtn.Visibility == Visibility.Visible)
            {
                if (ActiveCourseVM.DisableCourseWithdrawal())
                    CourseWithdrawalBtn.IsEnabled = false;
            }
        }

    }
}
