using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for CourseCreateWindow.xaml
    /// </summary>
    public partial class CourseCreateWindow : Window
    {
        public CourseDTO Course { get; set; }
        private CourseController courseController { get; set; }
        private ExamSlotController examController { get; set; }
        public CourseCreateWindow(AppController appController, Tutor loggedIn)
        {
            Course = new CourseDTO();
            Course.TutorId = loggedIn.Id;

            examController = appController.ExamSlotController;
            courseController = appController.CourseController;

            InitializeComponent();
            DataContext = this;
            languageLvlCb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            classsroomCb.IsChecked = false;
            maxNumOfStudentsTb.IsEnabled = false;
            mon.IsChecked = false;
            tue.IsChecked = false;
            wed.IsChecked = false;
            thu.IsChecked = false;
            fri.IsChecked = false;
            startDateDp.SelectedDate = DateTime.Now;
        }

        private void CourseCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Course.IsValid)
            {
                if(courseController.CanCreateOrUpdateCourse(Course.ToCourse(), examController))
                {
                    courseController.Add(Course.ToCourse());
                    MessageBox.Show("Success!");
                    Close();
                }
                else
                {
                    MessageBox.Show("The course cannot be created, there are time overlaps or no available classroms (if the course is held in a classroom).");
                }
            }
            else
            {
                MessageBox.Show("Something went wrong. Please check all fields in the form.");
            }

        }

        // Method enables textbox for maxNumOfStudents when the course is to be held in a classroom

        private void ClasssroomCb_Checked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = true;
            inClassroomErrorTb.Text = "Please enter the maximal number of students, as it is required";
        }

        private void ClasssroomCb_Unchecked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = false;
            inClassroomErrorTb.Text = "";
        }
    }
}
