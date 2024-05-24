using System.Windows;
using LangLang.WPF.ViewModels;

namespace LangLang
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel {  get; set; }
        public MainWindow()
        {
            ViewModel = new();
            DataContext = ViewModel;
            InitializeComponent();
            PenaltyPointReducer reducer = new PenaltyPointReducer();
            reducer.UpdatePenaltyPoints();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Login()) Close();
        }

        private void SignupBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowRegistrationWindow();
            Close();
        }
    }
}
