using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for ExamSlotCreateWindow.xaml
    /// </summary>
    public partial class ExamSlotCreateWindow : Window
    {
        public List<Course> Courses { get; set; }
        public Course SelectedCourse { get; set; }
        public ExamSlotDTO ExamSlot { get; set; }
        private ExamSlotController examSlotsController { get; set; }
        //public ExamSlotCreateWindow(Dictionary<int,Course> courses, ExamSlotController controller)
        public ExamSlotCreateWindow(Dictionary<int,Course> courses,ExamSlotController controller)
        {
            Courses = courses.Values.ToList<Course>();
            SelectedCourse = new Course();
            examSlotsController = controller;
            ExamSlot = new ExamSlotDTO();

            InitializeComponent();
            DataContext = this;

        }

        private void examSlotCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Mas "+ExamSlot.MaxStudents);
            if (ExamSlot.IsValid)
            {

                if (ExamSlot.CourseId == -1)
                {
                    MessageBox.Show("Must select course.");

                }
                else
                {
                    ExamSlot.CourseId = SelectedCourse.Id;
                    examSlotsController.Add(ExamSlot.ToExamSlot());
                    Close();
                }
                
            }
            else
            {
                if (ExamSlot.CourseId == -1)
                {
                    MessageBox.Show("Must select course.");

                }
                else
                {
                    MessageBox.Show("Exam slot can not be created. Not all fields are valid.");
                }
            }
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedCourse!= null)
            {
                //SelectedCourse = (Course)CoursesDataGrid.SelectedItem;
                languageTb.Text = SelectedCourse.Language;
                ExamSlot.Language = SelectedCourse.Language;
                levelTb.Text = SelectedCourse.Level.ToString();
                ExamSlot.Level = SelectedCourse.Level;
                ExamSlot.CourseId = SelectedCourse.Id;
                //CoursesDataGrid.SelectedItem = null;
            }
        }
    }
}
