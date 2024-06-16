using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.WPF.Views.StudentView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamApplicationsVM
    {
        private readonly StudentWindow _parent;
        public ExamApplicationViewModel SelectedApplication { get; set; }
        public ObservableCollection<ExamApplicationViewModel> Applications { get; set; }
        public List<ExamApplication> ApplicationsForReview { get; set; }

        private Student loggedIn;

        public ExamApplicationsVM(Student loggedIn, StudentWindow parent)
        {
            this._parent = parent;
            this.loggedIn = loggedIn;
            Applications = new();
            ApplicationsForReview = new();

            SetDataForReview();
        }

        public void SetDataForReview()
        {
            ExamApplicationService appService = new();
            ApplicationsForReview = appService.GetApplications(loggedIn);
        }
        public void Update()
        {
            Applications.Clear();
            SetDataForReview();
            foreach (ExamApplication application in ApplicationsForReview)
            {
                Applications.Add(new ExamApplicationViewModel(application));
            }
        }

        public void CancelApplication()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to cancel exam application?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ExamApplicationService appService = new();
                bool canceled = appService.CancelApplication(SelectedApplication.ToExamApplication());
                if (canceled)
                {
                    MessageBox.Show("Cancelation was successful.");
                    _parent.Update();
                }
                else
                    MessageBox.Show($"Can't cancel exam. There is less than {Constants.EXAM_CANCELATION_PERIOD} days or exam has passed");

            }
        }
    }
}
