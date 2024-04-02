using LangLang.Core.Controller;
using LangLang.Core.Model;

using LangLang.DTO;
using LangLang.View;
using LangLang.View.CourseGUI;
using LangLang.View.ExamSlotGUI;
using LangLang.Core.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Data;
using System.Reflection;
using System.Xml.Linq;
using System.Reflection.Emit;
using System.Diagnostics;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for TutorWindow.xaml
    /// </summary>
    public partial class TutorWindow : Window, IObserver
    {
        //for exam slots
        public ObservableCollection<ExamSlotDTO> ExamSlots { get; set; }
        public ExamSlotDTO SelectedExamSlot { get; set; }
        private ExamSlotController examSlotsController { get; set; }

        //for courses
        public ObservableCollection<CourseDTO> Courses { get; set; }
        public CourseDTO SelectedCourse { get; set; }
        private CourseController coursesController { get; set; }

        public Tutor tutor { get; set; }
        public TutorWindow()
        {
            //tutor = t;
            tutor = new Tutor();
            tutor.Profile.Id = 1;

            InitializeComponent();
            DataContext = this;

            ExamSlots = new ObservableCollection<ExamSlotDTO>();
            examSlotsController = new ExamSlotController();
            coursesController = new CourseController();
            Courses = new ObservableCollection<CourseDTO>();

            
            coursesController.Subscribe(this);
            examSlotsController.Subscribe(this);
            Update();
        }

        public void Update()
        {
            ExamSlots.Clear();
            //filter exam slots for this tutor
            foreach (ExamSlot exam in examSlotsController.GetAllExamSlots().Values)
            {
                Course c = coursesController.GetAllCourses()[exam.CourseId];
                ExamSlots.Add(new ExamSlotDTO(exam, c));
            }


            Courses.Clear();
            foreach (Course course in coursesController.GetCoursesByTutor(tutor).Values)
                Courses.Add(new CourseDTO(course));
            coursesTable.ItemsSource = Courses;
        }

        private void ExamSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            //fix to courses by tutor
            Trace.WriteLine("U tutorwindow " + coursesController.GetAllCourses().Count);

            ExamSlotCreateWindow examSlotCreateWindow = new ExamSlotCreateWindow(coursesController.GetAllCourses(), examSlotsController);
            examSlotCreateWindow.Show();
        }

        private void ExamSlotUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotUpdateWindow examSlotUpdateWindow = new ExamSlotUpdateWindow();
            examSlotUpdateWindow.Show();
        }

        private void CourseCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseCreateWindow courseCreateWindow = new CourseCreateWindow(coursesController);
            courseCreateWindow.Show();
        }

        private void ExamSlotDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("IDDDDD "+SelectedExamSlot.MaxStudents);
            if (!examSlotsController.Delete(SelectedExamSlot.Id))
            {
                MessageBox.Show("Can't delete exam, there is less than 14 days before exam.");
            }
            
            Update();
         }
        private void CourseUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
           // CourseUpdateWindow courseUpdateWindow = new CourseUpdateWindow(courseController);
            //courseUpdateWindow.Show();
        }

        private void coursesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ExamSlotSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotSearchWindow examSlotSearchWindow = new ExamSlotSearchWindow(coursesController, examSlotsController, tutor.Id);
            examSlotSearchWindow.Show();
        }
    }
}
