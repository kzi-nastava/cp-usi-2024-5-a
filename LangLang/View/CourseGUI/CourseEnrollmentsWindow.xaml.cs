﻿using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.RequestsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for CourseEnrollmentsWindow.xaml
    /// </summary>
    public partial class CourseEnrollmentsWindow : Window
    {
        public EnrollmentRequestViewModel SelectedEnrollment { get; set; }
        public ObservableCollection<EnrollmentRequestViewModel> Enrollments { get; set; }
        private AppController appController;
        private EnrollmentRequestService enrollmentReqService;
        private CourseController courseController;
        private TutorService tutorService;
        private CourseViewModel course;
        public CourseEnrollmentsWindow(AppController appController, CourseViewModel course)
        {
            InitializeComponent();
            DataContext = this;

            this.appController = appController;
            this.course = course;
            enrollmentReqService = new();
            courseController = appController.CourseController;
            tutorService = new();

            Enrollments = new();

            rejectBtn.IsEnabled = false;
            conifrmListBtn.IsEnabled = true;

            Update();
        }

        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableForm();
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to reject " + SelectedEnrollment.StudentName + " " + SelectedEnrollment.StudentLastName + " from the course?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                EnrollmentRequest enrollment = SelectedEnrollment.ToEnrollmentRequest();
                enrollment.UpdateStatus(Status.Rejected);
                enrollmentReqService.Update(enrollment);
                Update();
                InputPopupWindow inputPopup = new InputPopupWindow();
                inputPopup.ShowDialog();
                NotifyStudentAboutRejection(enrollment.StudentId, inputPopup.EnteredText);
                ShowSuccess();
            }
            conifrmListBtn.IsEnabled = true;
        }

        public void Update()
        {
            Enrollments.Clear();
            foreach (EnrollmentRequest enrollment in enrollmentReqService.GetByCourse(course.ToCourse()))
            {
                if(enrollment.Status == Status.Pending)
                {
                    Enrollments.Add(new EnrollmentRequestViewModel(enrollment));
                }
            }
        }

        private void AcceptListBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to confirm list?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // if the course is not online and the number of enrollments excedes the maximal number of students
                if(course.NotOnline && Enrollments.Count > course.ToCourse().MaxStudents)
                    MessageBox.Show("You have exceded the maximal number of students for this course.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                { 
                    foreach (EnrollmentRequestViewModel enrollmentDTO in Enrollments)
                    {
                        EnrollmentRequest enrollment = enrollmentDTO.ToEnrollmentRequest();
                        if(enrollment.Status != Core.Model.Enums.Status.Rejected)
                        {
                            enrollment.UpdateStatus(Status.Accepted);
                            enrollmentReqService.Update(enrollment);
                            NotifyStudentAboutAcceptence(enrollment.StudentId);
                        }
                    }
                    UpdateCourse(false, Enrollments.Count);
                    DisableForm();
                    ShowSuccess();
                    Close();
                }
            }
        }
        private void UpdateCourse(bool modifiable, int studentsCount)
        {
            course.Modifiable = modifiable;
            course.NumberOfStudents = studentsCount;
            courseController.Update(course.ToCourse());
        }
        private void NotifyStudentAboutAcceptence(int studentId)
        {
            var studentService = new StudentService();
            // Implement once the email sending functionality is added.

        }
        private void NotifyStudentAboutRejection(int studentId, string reason)
        {
            var studentService = new StudentService();
            // Implement once the email sending functionality is added.
        }
        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void EnrollmentTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedEnrollment != null && course.Modifiable)
                rejectBtn.IsEnabled = true;
            else
                rejectBtn.IsEnabled = false;
        }
        private void DisableForm()
        {
            rejectBtn.IsEnabled = false;
            conifrmListBtn.IsEnabled = false;
        }
    }
}
