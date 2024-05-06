using LangLang.Core.Controller;
using LangLang.DTO;
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
    /// Interaction logic for EnterGardesWindow.xaml
    /// </summary>
    public partial class EnterGradesWindow : Window
    {


        public EnterGradesWindow(AppController appController, CourseDTO course)
        {
            InitializeComponent();
        }

        private void StudentsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
