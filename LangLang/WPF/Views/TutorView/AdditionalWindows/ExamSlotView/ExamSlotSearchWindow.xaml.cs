using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModels;
using System;
using System.Windows;

namespace LangLang.WPF.Views.TutorView.AdditionalWindows.ExamSlotView
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
