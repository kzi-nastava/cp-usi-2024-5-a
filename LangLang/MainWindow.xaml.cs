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
        private TutorController tutorController { get; set; }
        private StudentController studentController { get; set; }
        private EnrollmentRequestController enrollmentRequestController { get; set; }
        private ExamSlotController examSlotController { get; set; } 
        private CourseController courseController { get; set; }

        private AppController appController { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            studentController = new();
            enrollmentRequestController = new();
            examSlotController = new();
            courseController = new();   
            appController = new();
            tutorController = new();
        }

        private void Tutor_Click(object sender, RoutedEventArgs e)
        {
            //PAZITI DA MORA DA POSTOJI SA KLJUCEM 7
            TutorWindow tutorWindow = new TutorWindow(tutorController.GetAllTutors()[7]);
            this.Visibility = Visibility.Hidden;
            tutorWindow.Show();
        }
        private void DirectorWindow(object sender, RoutedEventArgs e)
        {
            DirectorWindow window = new(appController.TutorController, new Director());
            window.Show();
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
