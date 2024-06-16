using LangLang.WPF.ViewModels.ExamViewModels;
using LangLang.WPF.Views.DirectorView.Tabs;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.DirectorView.AdditionalWindows
{
    public partial class ExamSlotCreateWindow : Window
    {
        public ExamSlotCreateVM ExamCreateVM { get; set; }
        private ExamSlotsReview _parent;
        public ExamSlotCreateWindow(ExamSlotsReview parent)
        {
            InitializeComponent();
            _parent = parent;
            ExamCreateVM = new ExamSlotCreateVM(null);
            DataContext = ExamCreateVM;
        }
        private void examSlotCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (languageTb.Text == "") return;

            if (ExamCreateVM.CreateExam())
            {
                _parent.Update();
                Close();
            }
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExamCreateVM.SelectedCourse == null) return;
            
            languageTb.Text = ExamCreateVM.SelectedCourse.Language;
            ExamCreateVM.ExamSlot.Language = ExamCreateVM.SelectedCourse.Language;
            levelTb.Text = ExamCreateVM.SelectedCourse.Level.ToString();
            ExamCreateVM.ExamSlot.Level = ExamCreateVM.SelectedCourse.Level;
            CoursesDataGrid.SelectedItem = null;
            
        }
    }
}
