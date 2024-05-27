using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.RequestsViewModel;
using LangLang.WPF.Views.StudentView;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.StudentView.Tabs
{
    public partial class EnrollmentRequests : UserControl
    {
        private StudentWindow parentWindow;
        public EnrollmentRequestsViewModel enrollmentRequestsVM {  get; set; }
        public EnrollmentRequests(Student currentlyLoggedIn, StudentWindow parentWindow)
        {
            InitializeComponent();
            enrollmentRequestsVM = new(currentlyLoggedIn, parentWindow);

            DataContext = enrollmentRequestsVM;

            this.parentWindow = parentWindow;
            SetDataForReview();
            CancelRequestBtn.IsEnabled = false;
        }


        public void SetDataForReview()
        {
            enrollmentRequestsVM.SetDataForReview();
        }

        private void CancelRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            enrollmentRequestsVM.CancelRequest();
        }

        private void EnrollmentRequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (enrollmentRequestsVM.EnableButton()) CancelRequestBtn.IsEnabled = true;
            else CancelRequestBtn.IsEnabled = false;
        }

        public void Update()
        {
            enrollmentRequestsVM.Update();
        }

    }
}
