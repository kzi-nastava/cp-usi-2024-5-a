using LangLang.Core.Model;
using LangLang.View;
using LangLang.View.CourseGUI;
using LangLang.View.ExamSlotGUI;
using LangLang.Core.Observer;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System;
using LangLang.Domain.Models;
using LangLang.BusinessLogic.UseCases;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.Configuration;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for TutorWindow.xaml
    /// </summary>
    public partial class TutorWindow : Window, IObserver
    {
        // EXAM SLOTS
        public ObservableCollection<ExamSlotViewModel> ExamSlots { get; set; }
        public ExamSlotViewModel SelectedExamSlot { get; set; }

        // COURSES
        public ObservableCollection<CourseViewModel> Courses { get; set; }
        public CourseViewModel SelectedCourse { get; set; }

        public Tutor LoggedIn { get; set; }
        public TutorWindow(Profile currentlyLoggedIn)
        {
            InitializeComponent();
            DataContext = this;

            TutorService tutorService = new();

            LoggedIn = tutorService.Get(currentlyLoggedIn.Id);
            ExamSlots = new ObservableCollection<ExamSlotViewModel>();
            Courses = new ObservableCollection<CourseViewModel>();

            DisableButtonsES();
            DisableButtonsCourse();
            CourseService courseService = new();
            courseService.Subscribe(this);
            ExamSlotService examSlotService = new();
            examSlotService.Subscribe(this);

            Update();
        }

        public void Update()
        {
            ExamSlots.Clear();
            ExamSlotService examSlotService = new();
            CourseService courseService = new();

            foreach (ExamSlot exam in examSlotService.GetExams(LoggedIn))
            {
                ExamSlots.Add(new ExamSlotViewModel(exam));
            }

            Courses.Clear();

            foreach (Course course in courseService.GetByTutor(LoggedIn))
            {
                Courses.Add(new CourseViewModel(course));
            }
        }

        private void ExamSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotCreateWindow createWindow = new (LoggedIn);
            createWindow.Show();
        }

        private void ExamSlotUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotService examSlotService = new();
            ExamSlotUpdateWindow updateWindow = new (SelectedExamSlot.Id, LoggedIn);
            if(examSlotService.CanBeUpdated(SelectedExamSlot.ToExamSlot()))
                updateWindow.Show();
            else
                MessageBox.Show($"Can't update exam, there is less than {Constants.EXAM_MODIFY_PERIOD} days before exam or exam has passed.");
        }
        private void ExamSlotDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotService examSlotService = new();
            if (!examSlotService.Delete(SelectedExamSlot.Id))
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
            CourseCreateWindow createWindow = new (LoggedIn);
            createWindow.Show();
        }

        private void CourseSearchWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseSearchWindow searchWindow = new (LoggedIn);
            searchWindow.Show();
        }

        
        private void CourseUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseService courseService = new();
            if (courseService.CanChange(SelectedCourse.Id))
            {
                CourseUpdateWindow updateWindow = new (SelectedCourse.Id);
                updateWindow.Show();
            }
            else
            {
                MessageBox.Show($"Selected course cannot be updated, it has already started or there are less than {Constants.COURSE_MODIFY_PERIOD} days before course start.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CourseDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseService courseService = new();
            if (courseService.CanChange(SelectedCourse.Id))
            {
                courseService.Delete(SelectedCourse.Id);
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
            CourseService courseService = new();
            if (courseService.GetEnd(SelectedCourse.ToCourse()) < DateTime.Now)
            {
                EnterGradesWindow gradesWindow = new(SelectedCourse);
                gradesWindow.Show();
            }
            else
            {
                MessageBox.Show("The course is not finished.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void DurationOfCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseService courseService = new();
            if (courseService.IsActive(SelectedCourse.ToCourse()))
            {
                DurationOfCourseWindow courseWindow = new(SelectedCourse);
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
                CourseEnrollmentsWindow enrollmentsWindow = new (SelectedCourse);
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
            ExamSlotSearchWindow searchWindow = new (LoggedIn);
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
            ExamSlotService examSlotService = new();
            if (examSlotService.ApplicationsVisible(SelectedExamSlot.Id) && SelectedExamSlot.Applicants != 0)
            {
                ExamApplications applicationsWindow = new(SelectedExamSlot);
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
                EnterResults resultsWindow = new(SelectedExamSlot);
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

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }
    }
}
