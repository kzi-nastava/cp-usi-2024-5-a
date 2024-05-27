using LangLang.WPF.ViewModels.ExamViewModels;
using System.Windows.Controls;

namespace LangLang.WPF.Views.DirectorView.Tabs
{

    public partial class ResultsSending : UserControl
    {
        public ExamResultsSendingViewModel ResultsSendingViewModel { get; set; }

        public ResultsSending()
        {
            InitializeComponent();
            ResultsSendingViewModel = new();
            DataContext = ResultsSendingViewModel;
            Update();
        }

        public void Update()
        {
            ResultsSendingViewModel.Update();
        }

        private void SendEmailBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ResultsSendingViewModel.SendResults();
        }

        private void ExamsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultsSendingViewModel.SelectedExam == null)
                SendEmailBtn.IsEnabled = false;
            else
                SendEmailBtn.IsEnabled = true;
        }

    }
}
