using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.ExamViewModel;
using System.Collections.Generic;
using System.Windows;
using LangLang.Configuration;
using System.Collections.ObjectModel;
using LangLang.WPF.ViewModels.CourseViewModels;

namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamSlotUpdateVM
    {
        public ObservableCollection<CourseViewModel> Courses { get; set; }
        private List<Course> courses { get; set; }
        public Course SelectedCourse { get; set; }
        public ExamSlotViewModel ExamSlot { get; set; }

        public ExamSlotUpdateVM(int selectedExamId, Tutor loggedIn)
        {
            Courses = new();
            SelectedCourse = new Course();
            ExamSlotService examSlotService = new();
            ExamSlot = new ExamSlotViewModel(examSlotService.Get(selectedExamId));
            CourseService courseService = new();
            courses = courseService.GetBySkills(loggedIn);
            Update();
        }

        public void Update()
        {
            Courses.Clear();
            foreach (var course in courses)
            {
                Courses.Add(new CourseViewModel(course));
            }
        }


        public bool UpdateExam()
        {
            if (ExamSlot.IsValid)
            {
                ExamSlotService examSlotService = new();
                if (!examSlotService.CanBeUpdated(ExamSlot.ToExamSlot()))
                {
                    MessageBox.Show($"Exam can not be updated. There is less than {Constants.EXAM_MODIFY_PERIOD} weeks left before the exam.");
                }
                else if (!examSlotService.CanCreateExam(ExamSlot.ToExamSlot()))
                {
                    MessageBox.Show($"Exam can not be updated. You must change exams date or time.");

                }
                else
                {
                    examSlotService.Update(ExamSlot.ToExamSlot());
                    MessageBox.Show($"Exam successfuly updated.");
                    return true;
                }


            }
            else
            {
                MessageBox.Show("Exam slot can not be updated. Not all fields are valid.");
            }
            return false;

        }

    }
}
