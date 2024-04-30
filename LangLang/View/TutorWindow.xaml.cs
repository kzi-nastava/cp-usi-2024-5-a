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

namespace LangLang
{
    /// <summary>
    /// Interaction logic for TutorWindow.xaml
    /// </summary>
    public partial class TutorWindow : Window, IObserver
    {
        // EXAM SLOTS
        public ObservableCollection<ExamSlotDTO> ExamSlots { get; set; }
        public ExamSlotDTO SelectedExamSlot { get; set; }
        private ExamSlotController examSlotController { get; set; }

        // COURSES
        public ObservableCollection<CourseDTO> Courses { get; set; }
        public CourseDTO SelectedCourse { get; set; }
        private CourseController courseController { get; set; }

        private AppController appController { get; set; }

        public Tutor LoggedIn { get; set; }
        public TutorWindow(AppController appController, Profile currentlyLoggedIn)
        {
            LoggedIn = appController.TutorController.GetAllTutors()[currentlyLoggedIn.Id];
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            examSlotController = appController.ExamSlotController;
            courseController = appController.CourseController;

            ExamSlots = new ObservableCollection<ExamSlotDTO>();
            Courses = new ObservableCollection<CourseDTO>();

            DisableButtonsES();
            DisableButtonsCourse();

            courseController.Subscribe(this);
            examSlotController.Subscribe(this);

            Update();
        }

        public void Update()
        {
            ExamSlots.Clear();

            foreach (ExamSlot exam in examSlotController.GetExams(LoggedIn))
            {
                ExamSlots.Add(new ExamSlotDTO(exam));
            }

            Courses.Clear();

            foreach (Course course in courseController.GetCourses(LoggedIn))
            {
                Courses.Add(new CourseDTO(course));
            }
        }

        private void ExamSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotCreateWindow createWindow = new (appController, LoggedIn);
            createWindow.Show();
        }

        private void ExamSlotUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotUpdateWindow updateWindow = new (appController, SelectedExamSlot.Id);
            updateWindow.Show();
        }

        private void CourseCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseCreateWindow createWindow = new (appController, LoggedIn);
            createWindow.Show();
        }

        private void CourseSearchWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseSearchWindow searchWindow = new (appController, LoggedIn);
            searchWindow.Show();
        }

        private void ExamSlotDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!examSlotController.Delete(SelectedExamSlot.Id))
            {
                MessageBox.Show("Can't delete exam, there is less than 14 days before exam.");
            }

            Update();
        }
        private void CourseUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (coursesController.CanCourseBeChanged(SelectedCourse.Id))
            {
                CourseUpdateWindow updateWindow = new (appController, SelectedCourse.Id);
                updateWindow.Show();
            }
            else
            {
                MessageBox.Show("Selected course cannot be updated, it has already started or there are less than 7 days before course start.");
            }
        }

        private void CourseDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (coursesController.CanCourseBeChanged(SelectedCourse.Id))
            {
                courseController.Delete(SelectedCourse.Id);
                Update();
                MessageBox.Show("The course has successfully been deleted.");
            }
            else
            {
                MessageBox.Show("Selected course cannot be deleted, it has already started or there are less than 7 days before course start.");
            }
        }

        private void EnterGradeBtn_Click(object sender, RoutedEventArgs e)
        {
            EnterGradesWindow gradesWindow = new ();
            gradesWindow.Show();
        }

        private void CourseEnrollmentBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: implement
        }
        private void CoursesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCourse == null)
            {
                DisableButtonsCourse();
            }
            else
            {
                EnableButtonsCourse();
            }
        }

        private void DisableButtonsCourse()
        {
            courseUpdateBtn.IsEnabled = false;
            courseDeleteBtn.IsEnabled = false;
        }

        private void EnableButtonsCourse()
        {
            courseUpdateBtn.IsEnabled = true;
            courseDeleteBtn.IsEnabled = true;
        }

        private void ExamSlotSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotSearchWindow searchWindow = new (appController, LoggedIn);
            searchWindow.Show();
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

        private void ButtonSeeStudentInfo_Click(object sender, RoutedEventArgs e)
        {
            ExamApplications applicationsWindow = new (appController);
            applicationsWindow.Show();
        }

        private void ButtonEnterResults_Click(object sender, RoutedEventArgs e)
        {
            EnterResults resultsWindow = new (appController);
            resultsWindow.Show();
        }

        private void ExamSlotsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedExamSlot == null) // when the DataGrid listener is triggered, check if there is a selection, and based on that, decide whether to enable or disable the buttons
            {
                DisableButtonsES();
            }
            else
            {
                EnableButtonsES();
            }
        }

    }
}
