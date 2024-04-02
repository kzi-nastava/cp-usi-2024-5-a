using LangLang.Core.Controller;
using System.Windows;
using LangLang.View;
using LangLang.Core.Model;
using System.Collections.Generic;
using System.Linq;
using LangLang.View.StudentGUI;

namespace LangLang
{
    public partial class MainWindow : Window
    {

        private AppController appController { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            appController = new();
        }

        private void Tutor_Click(object sender, RoutedEventArgs e)
        {
            TutorWindow tutorWindow = new TutorWindow();
            this.Visibility = Visibility.Hidden;
            tutorWindow.Show();
        }
        private void DirectorWindow(object sender, RoutedEventArgs e)
        {
            //DirectorWindow window = new();
            //window.Show();
        }

        private void RegisterWindow(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new(appController, appController.StudentController);
            registrationWindow.Show();
        }

        private void StudentWindow(object sender, RoutedEventArgs e)
        {
            
            StudentWindow studentWindow = new(appController, appController.StudentController, appController.StudentController.GetAllStudents()[appController.StudentController.GetAllStudents().Keys.Max()], appController.EnrollmentRequestController, appController.CourseController, appController.ExamSlotController);
            studentWindow.Show();
        }
    }
}
