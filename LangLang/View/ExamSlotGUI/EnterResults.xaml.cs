using System.Windows;
using System.Windows.Controls;


namespace LangLang.View.ExamSlotGUI
{
    /// <summary>
    /// Interaction logic for EnterResults.xaml
    /// </summary>
    public partial class EnterResults : Window
    {
        public EnterResults()
        {
            InitializeComponent();

            disableForm();
        }

        private void disableForm()
        {
            nameTB.IsEnabled = false;
            lastnameTB.IsEnabled = false;
            emailTB.IsEnabled = false;
            readingPointsTB.IsEnabled = false;
            listeningPointsTB.IsEnabled = false;
            writingPointsTB.IsEnabled = false;
            speakingPointsTB.IsEnabled = false;
            confirmResultBtn.IsEnabled = false;
        }

        private void enableForm()
        {
            readingPointsTB.IsEnabled = true;
            listeningPointsTB.IsEnabled = true;
            writingPointsTB.IsEnabled = true;
            speakingPointsTB.IsEnabled = true;
            confirmResultBtn.IsEnabled = true;
        }

        private void studentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: implement
        }

        private void fillForm()
        {

        }

    }
}
