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
        public List<Course> Skills { get; set; }
        public Course SelectedCourse { get; set; }
        public ExamSlotDTO ExamSlot { get; set; }
        private ExamSlotController examSlotsController { get; set; }
        public ExamSlotCreateWindow(AppController appController, Tutor loggedIn)
        {
            Skills = appController.CourseController.GetCoursesForSkills(loggedIn);
            SelectedCourse = new Course();
            examSlotsController = appController.ExamSlotController;
            ExamSlot = new ExamSlotDTO();
            ExamSlot.TutorId = loggedIn.Id;

            InitializeComponent();
            DataContext = this;

        }

        private void examSlotCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseController courseController = new CourseController();
            if (ExamSlot.IsValid)
            {
                
                if (SelectedCourse==null)
                {
                    MessageBox.Show("Must select language and level.");

                }
                else
                {
                    bool added = examSlotsController.Add(ExamSlot.ToExamSlot(), courseController);
                    
                    if (!added)
                    {
                        MessageBox.Show("Choose another exam date or time.");
                    }
                    else
                    {
                        MessageBox.Show("Exam successfuly created.");

                        Close();
                    }
                    
                }
                
            }
            else
            {
                
                if (SelectedCourse==null)
                {
                    MessageBox.Show("Must select language and level.");

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
                //ExamSlot.CourseId = SelectedCourse.Id;
                //CoursesDataGrid.SelectedItem = null;
            }
        }

    }
}
