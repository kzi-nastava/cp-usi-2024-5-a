using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using LangLang.View;
using LangLang.View.CourseGUI;
using LangLang.View.ExamSlotGUI;
using LangLang.Core.Observer;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for TutorWindow.xaml
    /// </summary>
    public partial class TutorWindow : Window, IObserver
    {
        //for exam slots
        public ObservableCollection<ExamSlotDTO> ExamSlots { get; set; }
        public ExamSlotDTO SelectedExamSlot { get; set; }
        private ExamSlotController examSlotsController { get; set; }

        //for courses
        public ObservableCollection<CourseDTO> Courses { get; set; }
        public CourseDTO SelectedCourse { get; set; }
        private CourseController coursesController { get; set; }

        private AppController appController { get; set; }

        public Tutor tutor { get; set; }
        public TutorWindow(AppController appController, Profile currentlyLoggedIn)
        {
            this.tutor = appController.TutorController.GetAllTutors()[currentlyLoggedIn.Id];
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            examSlotsController = appController.ExamSlotController;
            coursesController = appController.CourseController;

            ExamSlots = new ObservableCollection<ExamSlotDTO>();
            Courses = new ObservableCollection<CourseDTO>();

            disableButtonsES();
            disableButtonsCourse();

            coursesController.Subscribe(this);
            examSlotsController.Subscribe(this);

            Update();
        }

        public void Update()
        {
            ExamSlots.Clear();
            //filter exam slots for this tutor
            foreach (ExamSlot exam in examSlotsController.GetExamSlotsByTutor(tutor.Id, coursesController))
            {
                Course c = coursesController.GetAllCourses()[exam.CourseId];
                ExamSlots.Add(new ExamSlotDTO(exam, c));
            }

            Courses.Clear();
            foreach (Course course in coursesController.GetCoursesByTutor(tutor).Values)
                Courses.Add(new CourseDTO(course));
            coursesTable.ItemsSource = Courses;
        }

        private void ExamSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotCreateWindow examSlotCreateWindow = new ExamSlotCreateWindow(coursesController.GetCoursesByTutor(tutor), examSlotsController);
            examSlotCreateWindow.Show();
        }

        private void ExamSlotUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
                ExamSlotUpdateWindow examSlotUpdateWindow = new ExamSlotUpdateWindow(SelectedExamSlot, coursesController.GetCoursesByTutor(tutor), examSlotsController);
                examSlotUpdateWindow.Show();
        }

        private void CourseCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseCreateWindow courseCreateWindow = new CourseCreateWindow(coursesController, examSlotsController, tutor.Id);
            courseCreateWindow.ShowDialog();
            Update();
        }

        private void CourseSearchWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseSearchWindow courseSearchWindow = new CourseSearchWindow(coursesController, tutor.Id);
            courseSearchWindow.Show();
            Update();
        }

        private void ExamSlotDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!examSlotsController.Delete(SelectedExamSlot.Id))
            {
                MessageBox.Show("Can't delete exam, there is less than 14 days before exam.");
            }
            
            Update();
         }
        private void CourseUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (coursesController.IsCourseValid(SelectedCourse.Id))
            {
                CourseUpdateWindow courseUpdateWindow = new CourseUpdateWindow(coursesController, examSlotsController, SelectedCourse.Id);
                courseUpdateWindow.Show();
                Update();
            }
            else
            {
                MessageBox.Show("Selected course cannot be updated, it has already started or there are less than 7 days before course start.");
            }
        }

        private void CourseDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (coursesController.IsCourseValid(SelectedCourse.Id))
            {
                int id = SelectedCourse.Id;
                examSlotsController.DeleteExamSlotsByCourseId(id);
                coursesController.Delete(id);
                Update();
                MessageBox.Show("The course has successfully been deleted.");
            }
            else
            {
                MessageBox.Show("Selected course cannot be deleted, it has already started or there are less than 7 days before course start.");
            }
        }
        private void coursesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCourse == null) {
                disableButtonsCourse();
            } else
            {
                enableButtonsCourse();
            }
        }

        private void disableButtonsCourse()
        {
            courseUpdateBtn.IsEnabled = false;
            courseDeleteBtn.IsEnabled = false;
        }
        private void enableButtonsCourse()
        {
            courseUpdateBtn.IsEnabled = true;
            courseDeleteBtn.IsEnabled = true;
        }

        private void ExamSlotSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotSearchWindow examSlotSearchWindow = new ExamSlotSearchWindow(coursesController, examSlotsController, tutor.Id);
            examSlotSearchWindow.Show();
        }

        private void disableButtonsES()
        {
            deleteExamBtn.IsEnabled = false;
            updateExamBtn.IsEnabled = false;
            examApplicationBtn.IsEnabled = false;
            enterResultsBtn.IsEnabled = false;
        }

        private void enableButtonsES()
        {
            deleteExamBtn.IsEnabled = true;
            updateExamBtn.IsEnabled = true;
            examApplicationBtn.IsEnabled = true;
            enterResultsBtn.IsEnabled = true;
        }

        private void ButtonSeeStudentInfo_Click(object sender, RoutedEventArgs e)
        {
            ExamApplications applicationsWindow = new ExamApplications(appController);
            applicationsWindow.Show();
        }

        private void ButtonEnterResults_Click(object sender, RoutedEventArgs e)
        {
            // TODO: implement
        }

        private void ExamSlotsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedExamSlot == null) // when the DataGrid listener is triggered, check if there is a selection, and based on that, decide whether to enable or disable the buttons
            {
                disableButtonsES();
            } else {
                enableButtonsES();
            }
        }

    }
}
