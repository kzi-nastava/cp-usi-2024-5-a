using System.Windows;


namespace LangLang.WPF.Views.TutorView.AdditionalWindows.CourseView
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
