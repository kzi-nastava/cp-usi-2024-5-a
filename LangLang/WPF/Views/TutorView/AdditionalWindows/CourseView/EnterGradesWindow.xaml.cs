using System.Windows;
using System.Windows.Controls;
using LangLang.WPF.ViewModels.CourseViewModels;

namespace LangLang.WPF.Views.TutorView.AdditionalWindows.CourseView
{
    /// <summary>
    /// Interaction logic for EnterGradesWindow.xaml
    /// </summary>
    public partial class EnterGradesWindow : Window
    {
        public CourseGradesViewModel CourseGradesVM { get; set; }
        public EnterGradesWindow(CourseViewModel course)
        {
            InitializeComponent();
            CourseGradesVM = new(course);
            DataContext = CourseGradesVM;
            DisableForm();
            CourseGradesVM.Update();
        }

        private void EnableForm()
        {
            gradeBtn.IsEnabled = true;
        }
        private void DisableForm()
        {
            gradeBtn.IsEnabled = false;
        }
        private void StudentTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CourseGradesVM.SelectedStudent != null)
                EnableForm();
            else
                DisableForm();
        }
        private void GradeBtn_Click(object sender, RoutedEventArgs e)
        {
            GradeSelectionPopup activityPopup = new GradeSelectionPopup();
            activityPopup.ChangeLabelName("Enter activity garde");
            bool? activityResult = activityPopup.ShowDialog();

            GradeSelectionPopup knowledgePopup = new GradeSelectionPopup();
            knowledgePopup.ChangeLabelName("Enter knowledge garde");
            bool? knowledgeResult = knowledgePopup.ShowDialog();

            if (activityResult == true && knowledgeResult == true)
            {
                int? selectedActivityGrade = activityPopup.SelectedNumber;
                int? selectedKnowledgeGrade = knowledgePopup.SelectedNumber;

                if (selectedActivityGrade.HasValue && selectedKnowledgeGrade.HasValue)
                {
                    CourseGradesVM.Grade((int)selectedActivityGrade, (int)selectedKnowledgeGrade);
                }
                else
                {
                    MessageBox.Show("You have not selected at least one of the grades.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
