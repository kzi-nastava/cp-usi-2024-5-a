using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.ViewModels.ExamViewModels;
using LangLang.WPF.Views.TutorView.Tabs;
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
