using LangLang.Core.Controller;
using System.Windows;
using LangLang.View;
using LangLang.Core.Model;
using System.Collections.Generic;

namespace LangLang
{
    public partial class MainWindow : Window
    {
        private TutorController tutorController { get; set; }
        private StudentController studentController { get; set; }
        private EnrollmentRequestController enrollmentRequestController { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            studentController = new();
            enrollmentRequestController = new();
        }

        private void DirectorWindow(object sender, RoutedEventArgs e)
        {
            //DirectorWindow window = new();
            //this.Close();
            //window.Show();
        }

        private void RegisterWindow(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new(studentController);
            this.Close();
            registrationWindow.Show();
        }

        private void StudentWindow(object sender, RoutedEventArgs e)
        {

            StudentWindow studentWindow = new(studentController, studentController.GetAllStudents()[0], enrollmentRequestController);
            this.Close();
            studentWindow.Show();
        }
    }
}
