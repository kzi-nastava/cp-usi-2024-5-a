using LangLang.Core.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LangLang.Core.Controller;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TutorController tutorController { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Tutor_Click(object sender, RoutedEventArgs e)
        {
            TutorWindow tutorWindow = new TutorWindow();
            this.Visibility = Visibility.Hidden;
            tutorWindow.Show();
        }
        private void DirectorWindow(object sender, RoutedEventArgs e)
        {
            //DirectorWindow window = new DirectorWindow();
            //window.Show();
        }

    }
}
