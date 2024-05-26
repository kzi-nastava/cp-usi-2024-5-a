using System.Windows;
using System.Windows.Controls;
using LangLang.WPF.Views.DirectorView.Tabs;

namespace LangLang.WPF.Views.DirectorView
{
    public partial class DirectorWindow : Window
    {

        public DirectorWindow()
        {
            InitializeComponent();
            DataContext = this;
            GenerateTabs();
        }

        private void GenerateTabs()
        {
            var reviewTab = new TutorReview();
            AddTab("Tutor review", reviewTab);
            var reportsTab = new Reports();
            AddTab("Reports", reportsTab);
            var resultsTab = new ResultsSending();
            AddTab("Results Sending", resultsTab);
        }

        private void AddTab(string header, UserControl content)
        {
            TabItem tabItem = new()
            {
                Header = header,
                Content = content
            };
            tabControl.Items.Add(tabItem);
        }

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }

    }
}
