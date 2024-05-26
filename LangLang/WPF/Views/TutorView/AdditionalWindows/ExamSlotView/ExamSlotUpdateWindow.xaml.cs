using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModels;
using LangLang.WPF.Views.TutorView.Tabs;
using System.Windows;

namespace LangLang.View.ExamSlotGUI
{
    /// <summary>
    /// Interaction logic for ExamSlotUpdateWindow.xaml
    /// </summary>
    public partial class ExamSlotUpdateWindow : Window
    {
        public ExamSlotUpdateVM ExamUpdateVM { get; set; }
        private ExamsReview _parent;
        public ExamSlotUpdateWindow(int selectedExamId, Tutor loggedIn, ExamsReview parent)
        {
            
            
            InitializeComponent();
            _parent = parent;
            ExamUpdateVM = new ExamSlotUpdateVM(selectedExamId, loggedIn);
            DataContext = ExamUpdateVM;
            
        }
        private void examSlotUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ExamUpdateVM.UpdateExam())
            {
                _parent.Update();
                Close();
            }

        }

        
    }
}
