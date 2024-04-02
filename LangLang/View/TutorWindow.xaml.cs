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
            InitializeComponent();
            DataContext = this;
            ExamSlots = new ObservableCollection<ExamSlotDTO>();
            examSlotsController = new ExamSlotController();
            coursesController = new CourseController();

            Course c = new Course();
            c.Days = new List<DayOfWeek>();
            c.Language = "engleski";

            c.Level = LanguageLevel.A2;
            Course c1 = new Course();
            c1.Language = "ruski";
            c1.Level = LanguageLevel.C2;
            c1.Days = new List<DayOfWeek>();
            Trace.WriteLine(c.Id);

            coursesController.Add(c);
            coursesController.Add(c1);

            Trace.WriteLine("Posle "+c.Id);

            if (coursesController.GetAllCourses().Values.Count == 1)
            {
                Trace.WriteLine("IMAAAA");

            }
            ExamSlot es = new ExamSlot(1, c.Id, 10, DateTime.Now, 0);
            ExamSlotDTO dto = new ExamSlotDTO(es, c);
            ExamSlots.Add(dto);
            examSlotsController.Add(es);
            
            //filter exam slots for this tutor
            examSlotsController.Subscribe(this);
            Update();
        }

        public void Update()
        {
            ExamSlots.Clear();
            Trace.WriteLine("POSLE");

            foreach (ExamSlot exam in examSlotsController.GetAllExamSlots().Values)
            {
                
                Trace.WriteLine(exam.MaxStudents);

                Course c = coursesController.GetAllCourses()[exam.CourseId];
                ExamSlots.Add(new ExamSlotDTO(exam, c));
            }
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
            CourseCreateWindow courseCreateWindow = new CourseCreateWindow(tutor);
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
    }
}
