using LangLang.Core.Controller;
using LangLang.DTO;
using System.Windows;


namespace LangLang.View.ExamSlotGUI
{
    /// <summary>
    /// Interaction logic for ExamApplications.xaml
    /// </summary>
    public partial class ExamApplications : Window
    {
        public StudentDTO SelectedStudent { get; set; }
        public ExamApplications(AppController appController)
        {
            //InitializeComponent();

        }

        private void conifrmListBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: implement
        }

        private void disallowBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: implement
        }

    }
}
