using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.ViewModels.ExamViewModels;
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

namespace LangLang.View.ExamSlotGUI
{
    /// <summary>
    /// Interaction logic for ExamSlotSearchWindow.xaml
    /// </summary>
    public partial class ExamSlotSearchWindow : Window
    {
        public ExamSlotSearchVM ExamsSearchVM { get; set; }

        public ExamSlotSearchWindow(Tutor loggedIn)
        {
            
            
            InitializeComponent();
            ExamsSearchVM = new ExamSlotSearchVM(loggedIn);

            DataContext = ExamsSearchVM;
            levelExamcb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));

        }
        public void Update()
        {
            ExamsSearchVM.SetDataForView();
        }
        private void SearchExam_Click(object sender, RoutedEventArgs e)
        {
            string? language = languageExamtb.Text;
            LanguageLevel? level = null;
            if (levelExamcb.SelectedValue != null)
                level = (LanguageLevel)levelExamcb.SelectedValue;
            DateTime examDate = examdatePicker.SelectedDate ?? default;

            ExamsSearchVM.SearchExams(ExamsSearchVM.loggedIn, examDate, language, level);
        }

        private void ClearExam_Click(object sender, RoutedEventArgs e)
        {
            ExamsSearchVM.ClearExams();
        }
    }

    
}
