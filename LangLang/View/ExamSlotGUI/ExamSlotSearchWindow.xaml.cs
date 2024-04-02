﻿using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.DTO;
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

namespace LangLang.View.ExamSlotGUI
{
    /// <summary>
    /// Interaction logic for ExamSlotSearchWindow.xaml
    /// </summary>
    public partial class ExamSlotSearchWindow : Window
    {
        public ObservableCollection<ExamSlotDTO> ExamSlots { get; set; }
        private List<ExamSlot> examSlotsForReview;
        private CourseController courseController;
        private ExamSlotController examSlotController;
        private int tutorId;

        public ExamSlotSearchWindow(CourseController courseController,ExamSlotController examSlotController, int tutorId)
        {
            ExamSlots = new ObservableCollection<ExamSlotDTO>();
            examSlotsForReview = new List<ExamSlot>();
            //fix to get only by this tutor
            examSlotsForReview = examSlotController.GetExamSlotsByTutor(tutorId,courseController);

            foreach(ExamSlot exam in examSlotsForReview)
            {
                Course course = courseController.GetAllCourses()[exam.CourseId];
                ExamSlots.Add(new ExamSlotDTO(exam,course));
            }

            this.courseController = courseController;
            this.examSlotController = examSlotController;
            this.tutorId = tutorId;
            InitializeComponent();
            DataContext = this;
            levelExamcb.ItemsSource = Enum.GetValues(typeof(LanguageLevel));

        }
        public void Update()
        {

            ExamSlots.Clear();
            foreach (ExamSlot exam in examSlotsForReview)
            {
                Course c = courseController.GetAllCourses()[exam.CourseId];
                ExamSlots.Add(new ExamSlotDTO(exam, c));
            }
        }
        private void SearchExam_Click(object sender, RoutedEventArgs e)
        {
            string? language = languageExamtb.Text;
            LanguageLevel? level = null;
            if (levelExamcb.SelectedValue != null)
                level = (LanguageLevel)levelExamcb.SelectedValue;
            DateTime examDate = examdatePicker.SelectedDate ?? default;


            examSlotsForReview = examSlotController.SearchExamSlotsByTutor(tutorId, courseController, examDate, language, level); 
            Update();
        }

        private void ClearExam_Click(object sender, RoutedEventArgs e)
        {
            //fix to show all exam slots for current tutor
            //examSlotsForReview = examSlotController.GetExamSlotsByTutor(tutorId, courseController);
            examSlotsForReview = examSlotController.GetExamSlotsByTutor(tutorId,courseController);
            Update();
        }
    }

    
}