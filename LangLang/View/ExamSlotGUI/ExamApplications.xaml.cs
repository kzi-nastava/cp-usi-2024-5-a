using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
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
        public StudentDTO SelectedStudent { get; set; }
        public ObservableCollection<StudentDTO> Students { get; set; }
        private AppController appController;
        private ExamAppRequestController requestController;
        private ExamSlotController examSlotController;
        private StudentController studentController;
        private ExamSlotDTO examSlot;

        public ExamApplications(AppController appController, ExamSlotDTO examSlot)
        {
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            this.examSlot = examSlot;
            requestController = appController.ExamAppRequestController;
            examSlotController = appController.ExamSlotController;
            studentController = appController.StudentController;

            Students = new ObservableCollection<StudentDTO>();

            AdjustButtons();

            Update();
        }

        public void Update()
        {
            Students.Clear();

            foreach (Student student in requestController.GetExamRequests(examSlot.Id, studentController))
            {
                Students.Add(new StudentDTO(student));
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
                examSlotController.Update(examSlot.ToExamSlot());
                AdjustButtons();
                ShowSuccess();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete " + SelectedStudent.Name + " " + SelectedStudent.LastName  + " from the examination?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                studentController.Delete(SelectedStudent.Id, appController.EnrollmentRequestController);
                Update();
                ShowSuccess();
            }
        }

        // NOTE: think about moving to utils
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StudentsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedStudent != null)
                deleteBtn.IsEnabled = true;
            else
                deleteBtn.IsEnabled = false;
        }

    }
}
