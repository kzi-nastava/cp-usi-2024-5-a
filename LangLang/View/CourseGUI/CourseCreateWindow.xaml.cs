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
        private int tutorId { get; set; }
        public CourseCreateWindow(CourseController courseController, ExamSlotController exams, int tutorId)
        {
            Course = new CourseDTO();
            Course.TutorId = tutorId;
            this.courseController = new CourseController();
            this.courseController = courseController;
            examController = new ExamSlotController();
            examController = exams;
            this.tutorId = tutorId;
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
        }

        private void CourseCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Course.IsValid)
            {
                if (Course.NotOnline)
                {
                    if (courseController.CanCreateLiveCourse(Course.ToCourse(), examController.GetAllExamSlots().Values.ToList<ExamSlot>()))
                    {
                        courseController.Add(Course.ToCourse());
                        MessageBox.Show("Success!");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("There seem to be time overlaps.");
                    }
                }
                else
                {
                    if (courseController.CanCreateOnlineCourse(Course.ToCourse(), examController.GetExamSlotsByTutor(tutorId, courseController)))
                    {
                        courseController.Add(Course.ToCourse());
                        MessageBox.Show("Success!");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("There seem to be time overlaps.");
                    }
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
