using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Reflection.Metadata.Ecma335;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamSlotCreateVM
    {
        public List<Course> Skills { get; set; }
        public Course SelectedCourse { get; set; }
        public ExamSlotViewModel ExamSlot { get; set; }
        public ExamSlotCreateVM(Tutor loggedIn)
        {
            CourseService courseService = new();
            Skills = courseService.GetBySkills(loggedIn);
            SelectedCourse = null;

            ExamSlot = new ExamSlotViewModel();
            ExamSlot.ExamDate = DateTime.Now;
            ExamSlot.Modifiable = true;
            ExamSlot.TutorId = loggedIn.Id;

        }
        public bool CreateExam()
        {
            ExamSlotService examSlotService = new();
            if (ExamSlot.IsValid)
            {
                if (SelectedCourse == null) MessageBox.Show("Must select language and level.");
                else
                {
                    bool added = examSlotService.Add(ExamSlot.ToExamSlot());

                    if (!added) MessageBox.Show("Choose another exam date or time.");
                    else
                    {
                        MessageBox.Show("Exam successfuly created.");
                        return true;
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
