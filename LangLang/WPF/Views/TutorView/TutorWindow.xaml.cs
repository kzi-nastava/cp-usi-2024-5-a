using System.Windows;
using System.Windows.Controls;
using LangLang.Domain.Models;
using LangLang.BusinessLogic.UseCases;
using LangLang.WPF.Views.TutorView.Tabs;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for TutorWindow.xaml
    /// </summary>
    public partial class TutorWindow : Window
    {
        public Tutor LoggedIn { get; set; }
        public TutorWindow(Tutor tutor)
        {
            InitializeComponent();
            DataContext = this;
            TutorService tutorService = new();
            LoggedIn = tutorService.Get(tutor.Id);
            GenerateTabs();
        }

        private void GenerateTabs()
        {
            var coursesTab = new Courses(LoggedIn);
            AddTab("Courses", coursesTab);
            var examsTab = new ExamsReview(LoggedIn); ;
            AddTab("Exams", examsTab);
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
