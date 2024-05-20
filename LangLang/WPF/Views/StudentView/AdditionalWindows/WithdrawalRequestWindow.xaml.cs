using LangLang.WPF.ViewModels.TutorViewModels;
using System.Windows;

namespace LangLang.WPF.Views.StudentView.AdditionalWindows
{
    public partial class WithdrawalRequestWindow : Window
    {
        public WithdrawalReqPageViewModel viewModel {get; set;}
        public WithdrawalRequestWindow(int enrollmentRequestId)
        {
            InitializeComponent();
            viewModel = new(enrollmentRequestId);
            DataContext = viewModel;
            
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Submit();
            Close();
        }



    }
}
