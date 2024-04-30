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
    /// Interaction logic for CourseUpdateWindow.xaml
    /// </summary>
    public partial class CourseUpdateWindow : Window
    {
        public CourseDTO Course { get; set; }
        private CourseController courseController;
        private ExamSlotController examController { get; set; }
        public CourseUpdateWindow(CourseController courseControler, ExamSlotController exams, int courseId)
        {
            this.courseController = courseControler;
            Course = new CourseDTO(courseController.GetById(courseId));

            examController = new ExamSlotController();
            examController = exams;
            InitializeComponent();
            DataContext = this;
            languageLvlCb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        private void CourseUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Course.IsValid)
            {
                if (Course.NotOnline)
                {
                    /*
                    if (courseController.CanUpdateLiveCourse(Course.ToCourse(), examController.GetAllExams()))
                    {
                        courseController.Update(Course.ToCourse());
                        MessageBox.Show("Success!");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("There seem to be time overlaps.");
                    }
                    */
                }
                else
                {
                    /*
                    if (courseController.CanUpdateOnlineCourse(Course.ToCourse(), examController.GetExams(Course.TutorId)))
                    {
                        courseController.Update(Course.ToCourse());
                        MessageBox.Show("Success!");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("There seem to be time overlaps.");
                    }
                    */
                }
            }
            else
            {
                MessageBox.Show("Something went wrong. Please check all fields in registration form.");
            }

        }

        // Method enables textbox for maxNumOfStudents when the course is to be held in a classroom

        private void ClasssroomCb_Checked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = true;
        }

        private void ClasssroomCb_Unchecked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = false;
        }
    }
}
