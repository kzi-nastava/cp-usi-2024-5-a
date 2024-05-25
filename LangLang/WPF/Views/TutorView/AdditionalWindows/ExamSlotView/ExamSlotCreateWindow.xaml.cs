using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.ViewModels.ExamViewModels;
using LangLang.WPF.Views.DirectorView.Tabs;
using LangLang.WPF.Views.TutorView.Tabs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for ExamSlotCreateWindow.xaml
    /// </summary>
    public partial class ExamSlotCreateWindow : Window
    {
        public ExamSlotCreateVM ExamCreateVM { get; set; }
        private ExamsReview _parent;
        public ExamSlotCreateWindow (Tutor loggedIn, ExamsReview parent)
        {
            InitializeComponent();
            _parent = parent;
            ExamCreateVM = new ExamSlotCreateVM(loggedIn);
            DataContext = ExamCreateVM;

        }

        private void examSlotCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ExamCreateVM.CreateExam())
            {
                _parent.Update();
                Close();
            }
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ExamCreateVM.SelectedCourse!= null)
            {
                ExamCreateVM.SelectedCourse = (Course)CoursesDataGrid.SelectedItem;
                languageTb.Text = ExamCreateVM.SelectedCourse.Language;
                ExamCreateVM.ExamSlot.Language = ExamCreateVM.SelectedCourse.Language;
                levelTb.Text = ExamCreateVM.SelectedCourse.Level.ToString();
                ExamCreateVM.ExamSlot.Level = ExamCreateVM.SelectedCourse.Level;
                //ExamSlot.CourseId = SelectedCourse.Id;
                //CoursesDataGrid.SelectedItem = null;
            }
        }

    }
}
