using LangLang.Core.Controller;
using System.Windows;
using LangLang.View;
using LangLang.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace LangLang
{
    public partial class MainWindow : Window
    {
        private TutorController tutorController { get; set; }
        private StudentController studentController { get; set; }
        private EnrollmentRequestController enrollmentRequestController { get; set; }

        private CourseController courseController { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            studentController = new();
            enrollmentRequestController = new();
            courseController = new();   
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
            Registration registrationWindow = new(studentController);
            registrationWindow.Show();
        }

        private void StudentWindow(object sender, RoutedEventArgs e)
        {
            
            StudentWindow studentWindow = new(studentController, studentController.GetAllStudents()[studentController.GetAllStudents().Keys.Max()], enrollmentRequestController, courseController);
            studentWindow.Show();
        }
    }
}
