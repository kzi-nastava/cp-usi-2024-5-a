using LangLang.Core.Model;
using LangLang.View;
using LangLang.View.CourseGUI;
using LangLang.View.ExamSlotGUI;
using LangLang.Core.Observer;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System;
using LangLang.Domain.Models;
using LangLang.BusinessLogic.UseCases;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.ExamViewModel;
using LangLang.WPF.Views.DirectorView.Tabs;
using LangLang.WPF.Views.TutorView.Tabs;
using LangLang.Configuration;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for TutorWindow.xaml
    /// </summary>
    public partial class TutorWindow : Window
    {
        public Tutor LoggedIn { get; set; }
        public TutorWindow(Profile currentlyLoggedIn)
        {
            InitializeComponent();
            DataContext = this;
            TutorService tutorService = new();
            LoggedIn = tutorService.Get(currentlyLoggedIn.Id);
            GenerateTabs();
        }

        private void GenerateTabs()
        {
            var coursesTab = new Courses(LoggedIn);
            AddTab("Courses", coursesTab);
            //var examsTab = new Exams(LoggedIn); ;
            //AddTab("Exams", examsTab);
        }
        private void AddTab(string header, UserControl content)
        {
            TabItem tabItem = new()
            {
                Header = header,
                Content = content
            };
            tabControl.Items.Add(tabItem);
        }

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }
    }
}
