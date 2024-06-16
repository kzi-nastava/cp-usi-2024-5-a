using LangLang.Domain.Models;
using System.Collections.ObjectModel;
using LangLang.BusinessLogic.UseCases;
using System.Windows;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamApplicationsPageViewModel
    {

        public ObservableCollection<ExamApplicationViewModel> Applications { get; set; }
        public ExamApplicationViewModel SelectedApplication { get; set; }
        public ExamSlotViewModel ExamSlot { get; set; }

        public ExamApplicationsPageViewModel(ExamSlotViewModel selectedExamSlot)
        {
            SelectedApplication = new();
            Applications = new();
            ExamSlot = selectedExamSlot;
        }

        public void Update()
        {
            var applicationsService = new ExamApplicationService();

            Applications.Clear();

            foreach (ExamApplication application in applicationsService.GetApplications(ExamSlot.Id))
                Applications.Add(new ExamApplicationViewModel(application));
        }

        public void ConfirmApplications()
        {
            var examService = new ExamSlotService();
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to confirm list?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ExamSlot.Modifiable = false;
                examService.Update(ExamSlot.ToExamSlot());
                ShowSuccess();
            }
        }

        public void Delete()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete " + SelectedApplication.StudentName + " " + SelectedApplication.StudentLastName + " from the examination?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var studentService = new StudentService();
                Student student = studentService.Get(SelectedApplication.StudentId);
                studentService.Deactivate(student.Id);
                Update();
                ShowSuccess();
            }
        }

        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
