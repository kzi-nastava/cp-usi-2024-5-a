using LangLang.Domain.Enums;
using LangLang.WPF.ViewModels.StudentViewModels;
using System;
using System.Windows;

namespace LangLang.WPF.Views.StudentView
{
    public partial class Registration : Window
    {
        public RegistrationViewModel ViewModel {  get; set; }

        public Registration()
        {
            InitializeComponent();
            ViewModel = new();
            DataContext = ViewModel;
            gendercb.ItemsSource = Enum.GetValues(typeof(Gender));
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SignUp()) Close();
        }

    }
}
