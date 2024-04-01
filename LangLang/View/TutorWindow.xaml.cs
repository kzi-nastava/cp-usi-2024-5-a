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

namespace LangLang
{
    /// <summary>
    /// Interaction logic for TutorWindow.xaml
    /// </summary>
    public partial class TutorWindow : Window, IObserver
    {
        //for exam slots
        public ObservableCollection<ExamSlotDTO> ExamSlots { get; set; }
        public ObservableCollection<CourseDTO> Courses { get; set; }
        public ExamSlotDTO SelectedExamSlot { get; set; }
        public CourseDTO SelectedCourse { get; set; }
        private ExamSlotController examSlotsController { get; set; }
        private CourseController courseController;
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
            Courses = new ObservableCollection<CourseDTO>();
            courseController = new CourseController();


            /*
            ExamSlot es = new ExamSlot(1, 1, 10, DateTime.Now, 0);
            ExamSlotDTO dto = new ExamSlotDTO(es);
            ExamSlots.Add(dto);
            examSlotsController.Add(es);
            */
            List<DayOfWeek> days = new List<DayOfWeek>();
            days.Add(DayOfWeek.Monday);
            Course c = new Course(1, 1, "eng", LanguageLevel.A1, 4,days, true, 0, DateTime.Now, false);
            courseController.Add(c);
            Course e = new Course(2, 1, "spanish", LanguageLevel.A1, 4, days, true, 0, DateTime.Now, false);
            courseController.Add(e);
            courseController.Subscribe(this);
            examSlotsController.Subscribe(this);
            Update();
        }

        public void Update()
        {
            ExamSlots.Clear();
            foreach (ExamSlot exam in examSlotsController.GetAllExamSlots().Values)
                ExamSlots.Add(new ExamSlotDTO(exam));
            Courses.Clear();
            foreach (Course course in courseController.GetCoursesByTutor(tutor).Values)
                Courses.Add(new CourseDTO(course));
            coursesTable.ItemsSource = Courses;
        }

        private void ExamSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotCreateWindow examSlotCreateWindow = new ExamSlotCreateWindow();
            examSlotCreateWindow.Show();
        }

        private void ExamSlotUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotUpdateWindow examSlotUpdateWindow = new ExamSlotUpdateWindow();
            examSlotUpdateWindow.Show();
        }

        private void CourseCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseCreateWindow courseCreateWindow = new CourseCreateWindow(courseController);
            courseCreateWindow.Show();
        }

        private void CourseUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
           // CourseUpdateWindow courseUpdateWindow = new CourseUpdateWindow(courseController);
            //courseUpdateWindow.Show();
        }

        private void coursesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
