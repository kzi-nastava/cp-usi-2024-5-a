using LangLang.Domain.Models;
using LangLang.View;
using LangLang.View.ExamSlotGUI;
using LangLang.WPF.ViewModels.ExamViewModels;
using System.Windows;
using System.Windows.Controls;


namespace LangLang.WPF.Views.TutorView.Tabs
{
    /// <summary>
    /// Interaction logic for Exams.xaml
    /// </summary>
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
            ExamSlotCreateWindow createWindow = new(ExamsTutorVM.LoggedIn,this);
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
            {
                DisableButtonsES();
            }
            else
            {
                EnableButtonsES();
            }
        }
        private void ExamSlotSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotSearchWindow searchWindow = new(ExamsTutorVM.LoggedIn);
            searchWindow.Show();
        }

        

        private void ButtonSeeApplications_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (ExamSlotService.ApplicationsVisible(SelectedExamSlot.Id) && SelectedExamSlot.Applicants != 0)
            {
                ExamApplications applicationsWindow = new(appController, SelectedExamSlot);
                applicationsWindow.Show();
            }
            else
            {
                MessageBox.Show($"If there are applications, they can only be viewed {Constants.PRE_START_VIEW_PERIOD} days before exam and during the exam.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            */
        }

        private void ButtonEnterResults_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (SelectedExamSlot.ExamDate.AddHours(Constants.EXAM_DURATION) < DateTime.Now) // after the EXAM_DURATION-hour exam concludes, it is possible to open a window.
            {
                EnterResults resultsWindow = new(appController, SelectedExamSlot);
                resultsWindow.Show();
            }
            else
            {
                MessageBox.Show("This window can be opened once the exam is passed!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            */
        }

    }
}
