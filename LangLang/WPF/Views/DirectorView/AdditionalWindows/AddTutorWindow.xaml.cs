using LangLang.Domain.Enums;
using LangLang.WPF.ViewModels.TutorViewModels;
using System;
using System.Windows;

namespace LangLang.WPF.Views.DirectorView.Tabs
{
    public partial class AddTutorWindow : Window
    {
        public AddTutorPageViewModel AddTutorViewModel { get;set; }
        private TutorReview _parent;

        public AddTutorWindow(TutorReview parent)
        {
            InitializeComponent();

            _parent = parent;
            AddTutorViewModel = new();
            DataContext = AddTutorViewModel;

            PopulateComboBox();
        }

        private void AddTutor_Click(object sender, RoutedEventArgs e)
        {
            AddTutorViewModel.AddTutor();
            _parent.TutorReviewViewModel.SetDataForReview();
            _parent.Update();
        }

        private void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            AddTutorViewModel.AddSkill(languageTB.Text, (Level)levelCB.SelectedValue);
        }

        private void PopulateComboBox()
        {
            levelCB.ItemsSource = Enum.GetValues(typeof(Level));
            genderCB.ItemsSource = Enum.GetValues(typeof(Gender));
        }
    }
}
