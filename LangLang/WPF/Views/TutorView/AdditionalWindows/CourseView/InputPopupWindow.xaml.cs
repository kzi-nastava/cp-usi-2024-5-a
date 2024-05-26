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

namespace LangLang.WPF.Views.TutorView.AddidtionalWindows.CourseView
{
    /// <summary>
    /// Interaction logic for InputPopupWindow.xaml
    /// </summary>
    public partial class InputPopupWindow : Window
    {
        public InputPopupWindow()
        {
            InitializeComponent();
        }
        public string EnteredText { get; private set; }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            EnteredText = TextInput.Text;
            Close();
        }
    }
}
