using System.Windows;
using System.Windows.Controls;


namespace LangLang.WPF.Views.TutorView.AdditionalWindows.CourseView
{
    /// <summary>
    /// Interaction logic for GradeSelectionPopup.xaml
    /// </summary>
    public partial class GradeSelectionPopup : Window
    {
        public int? SelectedNumber { get; private set; }
        public GradeSelectionPopup()
        {
            InitializeComponent();
        }
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (gradeCb.SelectedItem != null)
            {
                SelectedNumber = int.Parse(((ComboBoxItem)gradeCb.SelectedItem).Content.ToString());
            }
            else
            {
                SelectedNumber = null;
            }

            DialogResult = true;
            Close();
        }
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ChangeLabelName(string newName)
        {
            lbl.Content = newName;
        }
    }
}
