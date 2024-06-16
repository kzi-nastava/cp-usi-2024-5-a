using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModels;
using LangLang.WPF.Views.TutorView.AdditionalWindows.ExamSlotView;
using System.Windows;
using System.Windows.Controls;


namespace LangLang.WPF.Views.TutorView.Tabs
{
    public partial class ExamsReview : UserControl
    {
        public ExamSlotsTutorViewModel ExamsTutorVM { get; set; }
        public ExamsReview(Tutor LoggedIn)
        {
            InitializeComponent();
            ExamsTutorVM = new ExamSlotsTutorViewModel(LoggedIn);
            DataContext = ExamsTutorVM;

            Update();
        }
        public void Update()
        {
            ExamsTutorVM.SetDataForReview();
        }
        private void ExamSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotCreateWindow createWindow = new(ExamsTutorVM.LoggedIn, this);
            createWindow.Show();
        }
        private void ExamSlotUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamsTutorVM.UpdateExam(this);
        }
        private void ExamSlotDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamsTutorVM.DeleteExam();
        }
        private void DisableButtonsES()
        {
            deleteExamBtn.IsEnabled = false;
            updateExamBtn.IsEnabled = false;
            examApplicationBtn.IsEnabled = false;
            enterResultsBtn.IsEnabled = false;
        }

        private void EnableButtonsES()
        {
            deleteExamBtn.IsEnabled = true;
            updateExamBtn.IsEnabled = true;
            examApplicationBtn.IsEnabled = true;
            enterResultsBtn.IsEnabled = true;
        }
        private void ExamSlotsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ExamsTutorVM.IsExamSelected()) // when the DataGrid listener is triggered, check if there is a selection, and based on that, decide whether to enable or disable the buttons
                DisableButtonsES();
            else
                EnableButtonsES();
        }
 
        private void ExamSlotSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotSearchWindow searchWindow = new(ExamsTutorVM.LoggedIn);
            searchWindow.Show();
        }

        private void ButtonSeeApplications_Click(object sender, RoutedEventArgs e)
        {
            ExamsTutorVM.SeeApplications();
        }

        private void ButtonEnterResults_Click(object sender, RoutedEventArgs e)
        {
            ExamsTutorVM.EnterResults();
        }

    }
}
