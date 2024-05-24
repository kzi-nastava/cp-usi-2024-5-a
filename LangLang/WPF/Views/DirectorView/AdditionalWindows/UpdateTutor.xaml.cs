using LangLang.Core.Model;
using LangLang.WPF.ViewModels.TutorViewModels;
using LangLang.WPF.Views.DirectorView.Tabs;
using System;
using System.Windows;

namespace LangLang.WPF.Views.DirectorView.AdditionalWindows
{

    public partial class UpdateTutor : Window
    {
        public UpdateTutorPageViewModel UpdateTutorViewModel { get; set; }
        private TutorReview _parent;

        public UpdateTutor(TutorViewModel tutor, TutorReview parent)
        {
            InitializeComponent();
            UpdateTutorViewModel = new(tutor);
            _parent = parent;
            DataContext = UpdateTutorViewModel;
            genderCB.ItemsSource = Enum.GetValues(typeof(Gender));
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateTutorViewModel.Update();
            _parent.Update();
        }

    }
}
