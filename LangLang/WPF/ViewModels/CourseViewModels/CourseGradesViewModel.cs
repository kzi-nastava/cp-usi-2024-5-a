using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.View.CourseGUI;
using LangLang.WPF.ViewModels.StudentViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CourseGradesViewModel
    {
        public StudentViewModel SelectedStudent { get; set; }
        public ObservableCollection<StudentViewModel> Students { get; set; }

        private CourseViewModel course;
        public CourseGradesViewModel(CourseViewModel course)
        {
            SelectedStudent = new();
            Students = new();
            this.course = course;
        }
        public void Update()
        {
            Students.Clear();
            EnrollmentRequestService enrollmentRequestService = new();
            WithdrawalRequestService withdrawalRequestService = new();
            GradeService gradeService = new();
            foreach (EnrollmentRequest enrollment in enrollmentRequestService.GetByCourse(course.ToCourse()))
            {
                // All studnets that attend the course (do not have accepted withdrawals)
                // and have not been graded
                if (enrollment.Status == Status.Accepted && !withdrawalRequestService.HasAcceptedWithdrawal(enrollment.Id))
                {
                    bool graded = false;
                    foreach (Grade grade in gradeService.GetByCourse(course.ToCourse()))
                    {
                        if (enrollment.StudentId == grade.StudentId)
                        {
                            graded = true;
                            break;
                        }
                    }
                    if (!graded)
                    {
                        var studentService = new StudentService();
                        Students.Add(new StudentViewModel(studentService.Get(enrollment.StudentId)));
                    }
                }
            }
        }

        public void Grade(int activityGrade, int knowledgeGrade)
        {
            GradeService gradeService = new();
            gradeService.Add(new Grade(0, course.Id, SelectedStudent.Id, activityGrade, knowledgeGrade));
            ShowSuccess();
            Update();
        }
        private void ShowSuccess()
        {
            MessageBox.Show("The student has been successfully graded", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
