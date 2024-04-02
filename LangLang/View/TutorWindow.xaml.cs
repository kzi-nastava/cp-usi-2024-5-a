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
            courseDeleteBtn.IsEnabled = false;
            courseUpdateBtn.IsEnabled = false;

            ExamSlots = new ObservableCollection<ExamSlotDTO>();
            examSlotsController = new ExamSlotController();
            coursesController = new CourseController();
            Courses = new ObservableCollection<CourseDTO>();

            List<DayOfWeek> days = new List<DayOfWeek>();
            days.Add(DayOfWeek.Monday);
            Course c = new Course(1, tutor.Id, "eng", LanguageLevel.A1, 4, days, true, 0, DateTime.Now, false);
            coursesController.Add(c);
            Course e = new Course(2, tutor.Id, "spanish", LanguageLevel.A2, 4, days, true, 0, DateTime.Now, false);
            coursesController.Add(e);

            Trace.WriteLine("Posle "+coursesController.GetAllCourses().Values.Count);

            if (coursesController.GetAllCourses().Values.Count == 1)
            {
                Trace.WriteLine("IMAAAA");
            Courses = new ObservableCollection<CourseDTO>();

            }
            ExamSlot es = new ExamSlot(1, c.Id, 10, DateTime.Now, 0);
            ExamSlotDTO dto = new ExamSlotDTO(es, c);
            ExamSlots.Add(dto);
            examSlotsController.Add(es);
           
            coursesController.Subscribe(this);
            examSlotsController.Subscribe(this);
            Update();
        }

        public void Update()
        {
            ExamSlots.Clear();
            Trace.WriteLine("POSLE");
            //filter exam slots for this tutor
            foreach (ExamSlot exam in examSlotsController.GetAllExamSlots().Values)
            {
                
                Trace.WriteLine(exam.MaxStudents);

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
            ExamSlotCreateWindow examSlotCreateWindow = new ExamSlotCreateWindow(coursesController.GetAllCourses(), examSlotsController);
            //ExamSlotCreateWindow examSlotCreateWindow = new ExamSlotCreateWindow(examSlotsController);

            examSlotCreateWindow.Show();
        }

        private void ExamSlotUpdateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotUpdateWindow examSlotUpdateWindow = new ExamSlotUpdateWindow();
            examSlotUpdateWindow.Show();
        }

        private void CourseCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseCreateWindow courseCreateWindow = new CourseCreateWindow(coursesController, tutor.Id);
            courseCreateWindow.ShowDialog();
            Update();
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
            if (coursesController.IsCourseValid(SelectedCourse.Id))
            {
                CourseUpdateWindow courseUpdateWindow = new CourseUpdateWindow(coursesController, SelectedCourse.Id);
                courseUpdateWindow.Show();
                Update();
            }
            else
            {
                MessageBox.Show("Selected course cannot be updated, it has already started or there are less than 7 days before course start.");
            }
        }

        private void CourseDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (coursesController.IsCourseValid(SelectedCourse.Id))
            {
                coursesController.Delete(SelectedCourse.Id);
                Update();
                MessageBox.Show("The course has successfully been deleted.");
            }
            else
            {
                MessageBox.Show("Selected course cannot be deleted, it has already started or there are less than 7 days before course start.");
            }
        }
        private void coursesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            courseUpdateBtn.IsEnabled = true;
            courseDeleteBtn.IsEnabled = true;
        }

    }
}
