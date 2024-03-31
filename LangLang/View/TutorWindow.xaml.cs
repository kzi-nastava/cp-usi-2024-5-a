using LangLang.View;
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

namespace LangLang
{
    /// <summary>
    /// Interaction logic for TutorWindow.xaml
    /// </summary>
    public partial class TutorWindow : Window
    {
        public TutorWindow()
        {
            InitializeComponent();
        }

        private void examSlotCreateWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ExamSlotCreateWindow examSlotCreateWindow = new ExamSlotCreateWindow();
            examSlotCreateWindow.Show();
        }
    }
}
