using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Controller;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View.ExamSlotGUI
{
    /// <summary>
    /// Interaction logic for ExamApplications.xaml
    /// </summary>
    public partial class ExamApplications : Window
    {
        public ExamApplicationViewModel SelectedApplication { get; set; }
        public ObservableCollection<ExamApplicationViewModel> Applications { get; set; }
        private ExamApplicationService applicationsService;
        private ExamSlotService examsService;
        private ExamSlotViewModel examSlot;

        public ExamApplications(ExamSlotViewModel examSlot)
        {
            InitializeComponent();
            DataContext = this;

            this.examSlot = examSlot;
            applicationsService = new();
            examsService = new();

            Applications = new();

            AdjustButtons();

            Update();
        }

        public void Update()
        {
            Applications.Clear();
            foreach (ExamApplication application in applicationsService.GetApplications(examSlot.Id))
            {
                Applications.Add(new ExamApplicationViewModel(application));
            }
        }

        private void AdjustButtons()
        {
            if (examSlot.Modifiable == false)
                confirmApplicationsBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

        private void ConfirmApplicationsBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to confirm list?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) {
                examSlot.Modifiable = false;
                examsService.Update(examSlot.ToExamSlot());
                AdjustButtons();
                ShowSuccess();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete " + SelectedApplication.StudentName + " " + SelectedApplication.StudentLastName  + " from the examination?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var studentService = new StudentService();
                Student student = studentService.Get(SelectedApplication.StudentId);
                studentService.Deactivate(student.Id);
                Update();
                ShowSuccess();
            }
        }

        // NOTE: think about moving to utils
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApplicationTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedApplication != null)
                deleteBtn.IsEnabled = true;
            else
                deleteBtn.IsEnabled = false;
        }

    }
}
