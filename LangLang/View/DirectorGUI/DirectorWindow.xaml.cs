﻿using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using LangLang.View.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View
{
    public partial class DirectorWindow : Window, IObserver
    {
        public ObservableCollection<TutorDTO> Tutors { get; set; }
        public TutorDTO SelectedTutor { get; set; }

        private TutorController tutorsController { get; set; }

        private Director currentyLoggedIn;

        private Dictionary<int, Tutor> data;

        public DirectorWindow(TutorController tutorController, Director currentyLoggedIn)
        {
            InitializeComponent();
            DataContext = this;
            Tutors = new ObservableCollection<TutorDTO>();
            this.tutorsController = tutorController;
            tutorsController.Subscribe(this);
            this.currentyLoggedIn = currentyLoggedIn;
            this.data = tutorController.GetAllTutors();
            Update();
            SetUpWindow();
        }


        public void Update()
        {
            Tutors.Clear();
            foreach (Tutor tutor in data.Values)
            {
                Tutors.Add(new TutorDTO(tutor));
            }
        }

        void SetUpWindow()
        {
            gendercb.ItemsSource = Enum.GetValues(typeof(UserGender));
            levelCB.ItemsSource = Enum.GetValues(typeof(LanguageLevel));
            ClearFields();
            DisableForm();
        }


        // open add new tutor window 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddTutorWindow window = new AddTutorWindow(tutorsController);
            window.Show();
        }

        // Update tutor
        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTutor != null)
            {
                SelectedTutor.Name = nameTB.Text;
                SelectedTutor.LastName = lastNameTB.Text;
                SelectedTutor.Email = emailtb.Text;
                SelectedTutor.BirthDate = BirthDatePicker.SelectedDate.Value;
                SelectedTutor.PhoneNumber = phonenumbertb.Text;
                SelectedTutor.Gender = (UserGender)gendercb.SelectedItem;
                SelectedTutor.Password = passwordTB.Text;
                SelectedTutor.EmploymentDate = EmploymentDatePicker.SelectedDate.Value;

                if (SelectedTutor.IsValid)
                {
                    tutorsController.Update(SelectedTutor.ToTutor());
                    MessageBox.Show("Successfully completed!");
                    ClearFields();
                    DisableForm();
                }
                else
                {
                    MessageBox.Show("Data is not valid");
                }

            }
            else
            {
                MessageBox.Show("Please choose a tutor to update!");
            }

        }

        // Delete tutor
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTutor == null)
            {
                MessageBox.Show("Please choose a tutor to delete!");
            }
            else
            {
                tutorsController.Delete(SelectedTutor.Id);
                MessageBox.Show("Tutor is successfully deleted");
                DisableForm();
                ClearFields();
            }
        }
        private void EnableForm()
        {
            nameTB.IsEnabled = true;
            lastNameTB.IsEnabled = true;
            emailtb.IsEnabled = true;
            phonenumbertb.IsEnabled = true;
            passwordTB.IsEnabled = true;
            gendercb.IsEnabled = true;
            BirthDatePicker.IsEnabled = true;
            EmploymentDatePicker.IsEnabled = true;
            confirmUpdateBtn.IsEnabled = true;
            confirmDeleteBtn.IsEnabled = true;
        }

        private void DisableForm()
        {
            nameTB.IsEnabled = false;
            lastNameTB.IsEnabled = false;
            emailtb.IsEnabled = false;
            phonenumbertb.IsEnabled = false;
            passwordTB.IsEnabled = false;
            gendercb.IsEnabled = false;
            BirthDatePicker.IsEnabled = false;
            EmploymentDatePicker.IsEnabled = false;
            confirmUpdateBtn.IsEnabled = false;
            confirmDeleteBtn.IsEnabled = false;
        }

        private void ClearFields()
        {
            nameTB.Text = string.Empty;
            lastNameTB.Text = string.Empty;
            emailtb.Text = string.Empty;
            phonenumbertb.Text = string.Empty;
            gendercb.SelectedIndex = -1;
            passwordTB.Text = string.Empty;
            BirthDatePicker.SelectedDate = null;
            EmploymentDatePicker.SelectedDate = null;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string language = languagetb.Text;
            DateTime date = datePickerEmployment.SelectedDate ?? default; // if picker s null, pick up a minVal
            LanguageLevel? level = null;
            if (levelCB.SelectedValue != null)
                level = (LanguageLevel)levelCB.SelectedValue;

            if (language == "" && date == DateTime.MinValue && level == null)
            {
                MessageBox.Show("Please select some criterion");
            }
            else
            {
                data = tutorsController.Search(languagetb.Text, date, level);
                Update();
            }


        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            data = tutorsController.GetAllTutors();
            Update();
        }

        private void TutorsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SelectedTutor != null)
            {
                EnableForm();
                nameTB.Text = SelectedTutor.Name;
                lastNameTB.Text = SelectedTutor.LastName;
                emailtb.Text = SelectedTutor.Email;
                phonenumbertb.Text = SelectedTutor.PhoneNumber;
                gendercb.SelectedItem = SelectedTutor.Gender;
                passwordTB.Text = SelectedTutor.Password;
                BirthDatePicker.SelectedDate = SelectedTutor.BirthDate;
                EmploymentDatePicker.SelectedDate = SelectedTutor.EmploymentDate;
                confirmUpdateBtn.IsEnabled = true;
                confirmDeleteBtn.IsEnabled = true;
                dataGrid.ItemsSource = SelectedTutor.LanguageLevel;
            }
        }

    }
}