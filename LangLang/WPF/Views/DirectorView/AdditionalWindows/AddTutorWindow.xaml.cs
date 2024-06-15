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
            DisableSkillForm();
            PopulateComboBox();
        }

        private void AddTutor_Click(object sender, RoutedEventArgs e)
        {
            if (AddTutorViewModel.AddTutor())
            {
                DisableTutorForm();
                EnableSkillForm();
            }
            _parent.TutorReviewViewModel.SetDataForReview();
            _parent.Update();
        }

        private void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            AddTutorViewModel.AddSkill();
        }

        private void PopulateComboBox()
        {
            levelCB.ItemsSource = Enum.GetValues(typeof(Level));
            genderCB.ItemsSource = Enum.GetValues(typeof(Gender));
        }

        private void DisableTutorForm()
        {
            nameTB.IsEnabled = false;
            lastNameTB.IsEnabled = false;
            emailTB.IsEnabled = false;
            phonenumbertb.IsEnabled = false;
            passwordtb.IsEnabled = false;
            genderCB.IsEnabled  = false;
            BirthDatePicker.IsEnabled = false;
            AddTutor.IsEnabled = false;
        }

        private void EnableSkillForm()
        {
            note.Visibility = Visibility.Visible;
            languageTB.IsEnabled = true;
            levelCB.IsEnabled = true;
            addLL.IsEnabled = true;
        }

        private void DisableSkillForm()
        {
            languageTB.IsEnabled = false;
            levelCB.IsEnabled = false;
            addLL.IsEnabled = false;
        }
    }
}
