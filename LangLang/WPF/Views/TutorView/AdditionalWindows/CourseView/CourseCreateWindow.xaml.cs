using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.TutorViewModels;
using LangLang.WPF.Views.TutorView.Tabs;
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
        public CreateCoursePageViewModel CreateCourseViewModel { get; set; }
        private Courses _parent;
        public CourseCreateWindow(Courses parent)
        {
            InitializeComponent();
            _parent = parent;
            CreateCourseViewModel = new();
            DataContext = CreateCourseViewModel;
            SetUpForm();
        }

        private void CourseCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CreateCourseViewModel.CreatedCourse())
            {
                _parent.Update();
                Close();
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

        private void SetUpForm()
        {
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
    }
}
