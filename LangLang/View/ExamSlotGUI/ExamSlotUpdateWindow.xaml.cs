using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
using System;
using System.Collections.Generic;
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

namespace LangLang.View.ExamSlotGUI
{
    /// <summary>
    /// Interaction logic for ExamSlotUpdateWindow.xaml
    /// </summary>
    public partial class ExamSlotUpdateWindow : Window
    {
        public List<Course> Courses { get; set; }
        public Course SelectedCourse { get; set; }
        public ExamSlotDTO ExamSlot { get; set; }
        private ExamSlotController examSlotsController { get; set; }
        public ExamSlotUpdateWindow(ExamSlotDTO selectedExamSlot, Dictionary<int, Course> courses, ExamSlotController controller)
        {
            Courses = courses.Values.ToList<Course>();
            SelectedCourse = new Course();
            SelectedCourse = courses[selectedExamSlot.CourseId];
            Trace.WriteLine("Id od kursa selektovanog " + SelectedCourse.Id);
            examSlotsController = controller;
            ExamSlot = new ExamSlotDTO();
            ExamSlot = selectedExamSlot;
            Trace.WriteLine("Id od Exam selektovanog " + ExamSlot.Id);

            //Prefill(ExamSlot);
            InitializeComponent();
            DataContext = this;
        }
        public void Prefill(ExamSlotDTO selectedExamSlot)
        {
            MaxStudentsTb.Text = selectedExamSlot.MaxStudents;
            languageTb.Text = selectedExamSlot.Language;
            levelTb.Text = selectedExamSlot.Level.ToString();
            ExamDateTb.Text = selectedExamSlot.ExamDate.ToString();
            ExamTimeTb.Text = selectedExamSlot.Time.ToString();
        }
        private void examSlotUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ExamSlot.IsValid)
            {
                examSlotsController.Update(ExamSlot.ToExamSlot());
                Close();

                /*
                if (!examSlotsController.Delete(ExamSlot.Id))
                {
                    MessageBox.Show("Can't update exam, there is less than 14 days before exam.");
                    return;
                }
                examSlotsController.Add(ExamSlot.ToExamSlot());
                Close();
                */
            }
            else
            {
                MessageBox.Show("Exam slot can not be created. Not all fields are valid.");
            }

        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCourse != null)
            {
                //SelectedCourse = (Course)CoursesDataGrid.SelectedItem;
                languageTb.Text = SelectedCourse.Language;
                ExamSlot.Language = SelectedCourse.Language;
                levelTb.Text = SelectedCourse.Level.ToString();
                ExamSlot.Level = SelectedCourse.Level;
                //CoursesDataGrid.SelectedItem = null;
            }
        }
    }
}
