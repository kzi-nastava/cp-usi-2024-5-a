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
using System;
using LangLang.Core;

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

        // MESSAGES
        public ObservableCollection<MessageDTO> Messages { get; set; }
        public MessageDTO SelectedMessage { get; set; }
        private MessageController messageController { get; set; }

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
            messageController = appController.MessageController;

            ExamSlots = new ObservableCollection<ExamSlotDTO>();
            Courses = new ObservableCollection<CourseDTO>();
            Messages = new ObservableCollection<MessageDTO>();

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

            Messages.Clear();

            foreach (Message message in messageController.GetReceivedMessages(LoggedIn))
            {
                Messages.Add(new MessageDTO(message, appController));
            }
        }

        private void ExamSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotCreateWindow createWindow = new (appController, LoggedIn);
            createWindow.Show();
        }

        private void ExamSlotUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotUpdateWindow updateWindow = new (appController, SelectedExamSlot.Id, LoggedIn);
            updateWindow.Show();
        }
        private void ExamSlotDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!examSlotController.Delete(SelectedExamSlot.Id))
            {
                MessageBox.Show($"Can't delete exam, there is less than {Constants.EXAM_MODIFY_PERIOD} days before exam.");
            }
            else
            {
                MessageBox.Show("Exam slot successfully deleted.");
            }

            Update();
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

        
        private void CourseUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (courseController.CanCourseBeChanged(SelectedCourse.Id))
            {
                CourseUpdateWindow updateWindow = new (appController, SelectedCourse.Id);
                updateWindow.Show();
            }
            else
            {
                MessageBox.Show($"Selected course cannot be updated, it has already started or there are less than {Constants.COURSE_MODIFY_PERIOD} days before course start.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CourseDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (courseController.CanCourseBeChanged(SelectedCourse.Id))
            {
                courseController.Delete(SelectedCourse.Id);
                Update();
                MessageBox.Show("The course has been successfully deleted.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Selected course cannot be deleted, it has already started or there are less than {Constants.COURSE_MODIFY_PERIOD} days before course start.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EnterGradeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(courseController.GetCourseEnd(SelectedCourse.ToCourse()) < DateTime.Now)
            {
                EnterGradesWindow gradesWindow = new(appController, SelectedCourse);
                gradesWindow.Show();
            }
            else
            {
                MessageBox.Show("The course is not finished.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void DurationOfCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (courseController.IsActive(SelectedCourse.ToCourse()))
            {
                DurationOfCourseWindow courseWindow = new(appController, SelectedCourse);
                courseWindow.Show();
            }
            else
            {
                MessageBox.Show("The course is not active.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void CourseEnrollmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCourse.Modifiable)
            {
                CourseEnrollmentsWindow enrollmentsWindow = new (appController, SelectedCourse);
                enrollmentsWindow.Show();
            }
            else
            {
                MessageBox.Show("The enrollments for this course have already been confirmed. No further changes are allowed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
            courseEnrollmentBtn.IsEnabled = false;
            courseEnterGradeBtn.IsEnabled = false;
            courseDurationBtn.IsEnabled = false;
        }

        private void EnableButtonsCourse()
        {
            courseUpdateBtn.IsEnabled = true;
            courseDeleteBtn.IsEnabled = true;
            courseEnrollmentBtn.IsEnabled = true;
            courseEnterGradeBtn.IsEnabled = true;
            courseDurationBtn.IsEnabled = true;
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

        private void ButtonSeeApplications_Click(object sender, RoutedEventArgs e)
        {
            if (examSlotController.ApplicationsVisible(SelectedExamSlot.Id) && SelectedExamSlot.Applicants != 0)
            {
                ExamApplications applicationsWindow = new(appController, SelectedExamSlot);
                applicationsWindow.Show();
            }
            else
            {
                MessageBox.Show($"If there are applications, they can only be viewed {Constants.PRE_START_VIEW_PERIOD} days before exam and during the exam.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonEnterResults_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedExamSlot.ExamDate.AddHours(Constants.EXAM_DURATION) < DateTime.Now) // after the EXAM_DURATION-hour exam concludes, it is possible to open a window.
            {
                EnterResults resultsWindow = new(appController, SelectedExamSlot);
                resultsWindow.Show();
            }
            else
            {
                MessageBox.Show("This window can be opened once the exam is passed!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
