using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.CourseViewModels;
using LangLang.WPF.ViewModels.ExamViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamSlotCreateVM
    {
        public ObservableCollection<CourseViewModel> Courses { get; set; }
        private List<Course> courses {  get; set; }
        public CourseViewModel SelectedCourse { get; set; }
        public ExamSlotViewModel ExamSlot { get; set; }
        private Tutor loggedId {  get; set; }
        private CourseService courseService {  get; set; }
        public ExamSlotCreateVM(Tutor loggedIn)
        {
            ExamSlot = new ExamSlotViewModel();
            Courses = new();
            this.loggedId = loggedIn;
            courseService = new CourseService();
            ExamSlot.ExamDate = DateTime.Now;
            ExamSlot.Modifiable = true;
            Update();
        }

        public void Update()
        {
            Courses.Clear();
            if (loggedId == null)
            {
                courses = courseService.GetAll();
                ExamSlot.TutorId = -1;
            }
            else
            {
                courses = courseService.GetBySkills(loggedId);
                ExamSlot.TutorId = loggedId.Id;
            }
            foreach (var course in courses)
            {
                Courses.Add(new CourseViewModel(course));
            }
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
                    if (loggedId == null)
                    {
                        ExamSlot.TutorId = SmartSystem.GetTutorForExam(ExamSlot.ToExamSlot());
                    }
                    else ExamSlot.TutorId = loggedId.Id;

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
