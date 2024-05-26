using System;
using System.Collections.Generic;
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

namespace LangLang.View.CourseGUI
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
