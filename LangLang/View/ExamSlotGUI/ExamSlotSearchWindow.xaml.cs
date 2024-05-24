using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
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
        public ObservableCollection<ExamSlotViewModel> ExamSlots { get; set; }
        private List<ExamSlot> examSlotsForReview;
        private Tutor loggedIn;

        public ExamSlotSearchWindow(Tutor loggedIn)
        {
            ExamSlots = new ObservableCollection<ExamSlotViewModel>();
            examSlotsForReview = new List<ExamSlot>();
            this.loggedIn = loggedIn;
            ExamSlotService examsService = new();
            examSlotsForReview = examsService.GetExams(loggedIn);

            foreach(ExamSlot exam in examSlotsForReview)
            {
                ExamSlots.Add(new ExamSlotViewModel(exam));
            }

            InitializeComponent();
            DataContext = this;
            levelExamcb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));

        }
        public void Update()
        {

            ExamSlots.Clear();
            foreach (ExamSlot exam in examSlotsForReview)
            {
                ExamSlots.Add(new ExamSlotViewModel(exam));
            }
        }
        private void SearchExam_Click(object sender, RoutedEventArgs e)
        {
            string? language = languageExamtb.Text;
            LanguageLevel? level = null;
            if (levelExamcb.SelectedValue != null)
                level = (LanguageLevel)levelExamcb.SelectedValue;
            DateTime examDate = examdatePicker.SelectedDate ?? default;

            ExamSlotService examsService = new();
            examSlotsForReview = examsService.SearchByTutor(loggedIn, examDate, language, level); 
            Update();
        }

        private void ClearExam_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotService examsService = new();
            examSlotsForReview = examsService.GetExams(loggedIn);
            Update();
        }
    }

    
}
