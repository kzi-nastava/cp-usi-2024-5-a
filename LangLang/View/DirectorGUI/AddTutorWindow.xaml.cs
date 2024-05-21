using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.WPF.ViewModels.TutorViewModels;
using System;
using System.ComponentModel;
using System.Windows;


namespace LangLang.View
{
    public partial class AddTutorWindow : Window, INotifyPropertyChanged
    {
        private TutorService tutorService;
        private AppController appController;
        public TutorViewModel Tutor { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public AddTutorWindow(AppController appController)
        {
            InitializeComponent();
            this.appController = appController;
            tutorService = new();
            Tutor = new TutorViewModel();
            DataContext = this;

            genderCB.ItemsSource = Enum.GetValues(typeof(Gender));
            levelCB.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
        }

        // add tutor 
        private void AddTutor_Click(object sender, RoutedEventArgs e)
        {
            Tutor.EmploymentDate = DateTime.Now; // set up employment date
            Tutor.Role = UserType.Tutor;
            if (Tutor.IsValid)
            {
                var profileService = new ProfileService();
                if (profileService.EmailExists(Tutor.Email)) MessageBox.Show("Email already exists. Try with a different email address.");
                else {
                    tutorService.Add(Tutor.ToTutor());
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Fields are not valid.");
            }
        }

        private void addLL_Click(object sender, RoutedEventArgs e)
        {
            string language = languageTB.Text;
            string selectedLevel = levelCB.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(language))
            {
                Tutor.Language.Add(language);
                Tutor.Level.Add((LanguageLevel)Enum.Parse(typeof(LanguageLevel), selectedLevel));

                MessageBox.Show("Skill is added.");

                LLDataGrid.ItemsSource = null;
                LLDataGrid.ItemsSource = Tutor.LanguageLevel;
            }
            else
            {
                MessageBox.Show("Please choose language and level.");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
