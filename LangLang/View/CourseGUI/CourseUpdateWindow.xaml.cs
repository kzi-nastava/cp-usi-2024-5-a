using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.WPF.ViewModels.CourseViewModels;
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
        public CourseViewModel Course { get; set; }
        private CourseController courseController;
        private ExamSlotController examController { get; set; }
        public CourseUpdateWindow(AppController appController, int courseId)
        {
            InitializeComponent();

            examController = appController.ExamSlotController;
            courseController = appController.CourseController;

            Course = new CourseViewModel(courseController.Get(courseId));

            DataContext = this;
            languageLvlCb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        private void CourseUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Course.IsValid)
            {
                if(courseController.CanCreateOrUpdate(Course.ToCourse(), examController))
                {
                    courseController.Update(Course.ToCourse());
                    MessageBox.Show("Success!");
                    Close();
                }
                else
                {
                    MessageBox.Show("The course cannot be updated, there are time overlaps or no available classroms (if the course is held in a classroom).");
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
