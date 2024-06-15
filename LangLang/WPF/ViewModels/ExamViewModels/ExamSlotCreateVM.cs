﻿using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.ExamViewModel;
using System;
using System.Collections.Generic;
using System.Windows;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamSlotCreateVM
    {
        public List<Course> Skills { get; set; }
        public CourseViewModel SelectedCourse { get; set; }
        public ExamSlotViewModel ExamSlot { get; set; }
        public ExamSlotCreateVM(Tutor loggedIn)
        {
            CourseService courseService = new();
            ExamSlot = new ExamSlotViewModel();
            if (loggedIn == null)
            {
                Skills = courseService.GetAll();
                ExamSlot.TutorId = -1;
            }
            else
            {
                Skills = courseService.GetBySkills(loggedIn);
                ExamSlot.TutorId = loggedIn.Id;
            }
            SelectedCourse = null;
            ExamSlot.ExamDate = DateTime.Now;
            ExamSlot.Modifiable = true;

        }
        public bool CreateExam()
        {
            ExamSlot.CreatedAt = DateTime.Now;
            ExamSlotService examSlotService = new();
            if (ExamSlot.IsValid)
            {
                if (SelectedCourse == null) MessageBox.Show("Must select language and level.");
                else if(ExamSlot.TutorId == -1)
                {
                    ExamSlot.TutorId = SmartSystem.GetTutorForExam(ExamSlot.ToExamSlot());
                    if(ExamSlot.TutorId != -1)
                    {
                        examSlotService.Add(ExamSlot.ToExamSlot());
                        MessageBox.Show("Exam successfuly created.");
                        return true;
                    }
                    MessageBox.Show("There are no suitable tutors for selected parameters.");
                }
                else
                {
                    try {
                        examSlotService.Add(ExamSlot.ToExamSlot());
                        MessageBox.Show("Exam successfuly created.");
                        return true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
            else
            {

                if (SelectedCourse == null) MessageBox.Show("Must select language and level.");
                else MessageBox.Show("Exam slot can not be created. Not all fields are valid.");

            }
            return false;
        }
        
    }
}
