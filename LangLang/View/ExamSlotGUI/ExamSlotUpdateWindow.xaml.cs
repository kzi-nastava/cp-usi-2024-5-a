using LangLang.BusinessLogic.UseCases;
using LangLang.Core;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
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
        public List<Course> Skills { get; set; }
        public Course SelectedCourse { get; set; }
        public ExamSlotViewModel ExamSlot { get; set; }

        public ExamSlotUpdateWindow(int selectedExamId, Tutor loggedIn)
        {
            
            //Courses = courses.Values.ToList<Course>();
            SelectedCourse = new Course();
            ExamSlotService examSlotService = new();
            ExamSlot = new ExamSlotViewModel(examSlotService.Get(selectedExamId));
            CourseService courseService = new();
            Skills = courseService.GetBySkills(loggedIn);

            //Prefill(ExamSlot);
            InitializeComponent();
            DataContext = this;
            
        }
        public void Prefill(ExamSlotViewModel selectedExamSlot)
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
                ExamSlotService examSlotService = new();
                if (!examSlotService.CanBeUpdated(ExamSlot.ToExamSlot()))
                {
                    MessageBox.Show($"Exam can not be updated. There is less than {Constants.EXAM_MODIFY_PERIOD} weeks left before the exam.");
                }else if (!examSlotService.CanCreateExam(ExamSlot.ToExamSlot()))
                {
                    MessageBox.Show($"Exam can not be updated. You must change exams date or time.");

                }
                else
                {
                    examSlotService.Update(ExamSlot.ToExamSlot());
                    MessageBox.Show($"Exam successfuly updated.");
                    Close();
                }


            }
            else
            {
                MessageBox.Show("Exam slot can not be updated. Not all fields are valid.");
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
                //ExamSlot.CourseId = SelectedCourse.Id;
                //CoursesDataGrid.SelectedItem = null;
            }
        }
    }
}
