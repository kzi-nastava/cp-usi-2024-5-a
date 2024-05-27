using LangLang.Domain.Models;
using LangLang.Domain.Enums;
using LangLang.WPF.ViewModels.StudentViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.WPF.Views.StudentView.Tabs
{
    public partial class DataTab : UserControl
    {
        private StudentWindow parentWindow {  get; set; }
        public StudentDataViewModel StudentDataViewModel {  get; set; }

        public DataTab(Student currentlyLoggedIn, StudentWindow studentView)
        {
            InitializeComponent();
            StudentDataViewModel = new(currentlyLoggedIn);
            DataContext = StudentDataViewModel;
            parentWindow = studentView;
            FillData();
            DisableForm();
        }

        private void EnableForm()
        {
            nametb.IsEnabled = true;
            lastnametb.IsEnabled = true;
            emailtb.IsEnabled = true;
            numbertb.IsEnabled = true;
            gendercb.IsEnabled = true;
            passwordtb.IsEnabled = true;
            birthdp.IsEnabled = true;
            professiontb.IsEnabled = true;
            gendercb.ItemsSource = Enum.GetValues(typeof(Gender));
        }

        private void DisableForm()
        {
            nametb.IsEnabled = false;
            lastnametb.IsEnabled = false;
            emailtb.IsEnabled = false;
            numbertb.IsEnabled = false;
            gendercb.IsEnabled = false;
            passwordtb.IsEnabled = false;
            birthdp.IsEnabled = false;
            professiontb.IsEnabled = false;
        }


        private void EditMode()
        {
            savebtn.Visibility = Visibility.Visible;
            discardbtn.Visibility = Visibility.Visible;
        }

        private void NormalMode()
        {
            savebtn.Visibility = Visibility.Collapsed;
            discardbtn.Visibility = Visibility.Collapsed;
        }

        private void FillData()
        {
            NormalMode();
            DisableForm();
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!StudentDataViewModel.Edit()) return;
            
            EnableForm();
            EditMode();
            
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!StudentDataViewModel.Save()) return;
            NormalMode();
            DisableForm();
        }

        private void DiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            StudentDataViewModel.DiscardDataChanges();
            FillData();
            NormalMode();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            StudentDataViewModel.Delete();
            parentWindow.Close();
        }

    }
}
