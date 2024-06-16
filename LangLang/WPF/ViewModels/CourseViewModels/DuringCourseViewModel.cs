using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.WPF.ViewModels.RequestViewModels;
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
    public class DuringCourseViewModel
    {
        public StudentViewModel SelectedStudent { get; set; }
        public WithdrawalRequestViewModel SelectedWithdrawal { get; set; }
        public ObservableCollection<StudentViewModel> Students { get; set; }
        public ObservableCollection<WithdrawalRequestViewModel> Withdrawals { get; set; }

        private CourseViewModel course;
        
        public DuringCourseViewModel(CourseViewModel course)
        {
            Students = new();
            Withdrawals = new();
            this.course = course;
        }
        public void Update()
        {
            Students.Clear();

            EnrollmentRequestService enrollmentRequestService = new();
            WithdrawalRequestService withdrawalRequestService = new();
            //All studnets that attend the course and do not have accepted withdrawals
            foreach (EnrollmentRequest enrollment in enrollmentRequestService.GetByCourse(course.ToCourse()))
            {
                if (enrollment.Status == Status.Accepted && !withdrawalRequestService.HasAcceptedWithdrawal(enrollment.Id))
                {
                    var studentService = new StudentService();
                    Students.Add(new StudentViewModel(studentService.Get(enrollment.StudentId)));
                }
            }
            Withdrawals.Clear();
            foreach (WithdrawalRequest withdrawal in withdrawalRequestService.GetByCourse(course.ToCourse()))
            {
                if (withdrawal.Status == Status.Pending)
                {
                    Withdrawals.Add(new WithdrawalRequestViewModel(withdrawal));
                }
            }
        }
        
        public void GivePenaltyPoint()
        {
            PenaltyPointService penaltyPointService = new();
            TutorService tutorService = new();
            if (penaltyPointService.HasGivenPenaltyPoint(SelectedStudent.ToStudent(), tutorService.Get(course.TutorId), course.ToCourse()))
                MessageBox.Show("You have already given the student a penalty point today.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                penaltyPointService.GivePenaltyPoint(SelectedStudent.ToStudent(), tutorService.Get(course.TutorId), course.ToCourse());
                NotifyStudentAboutPenaltyPoint(SelectedStudent.Id);
            }
            Update();
        }
        private void NotifyStudentAboutPenaltyPoint(int studentId)
        {
            var studentService = new StudentService();
            var student = studentService.Get(studentId);
            var mailMessage = $"You have received one penalty point in course: {course.Language}, level: {course.Level}";

            EmailService.SendEmail(student.Profile.Email, "Penalty point", mailMessage);
        }
        public void AcceptWithdrawal()
        {
            WithdrawalRequestService withdrawalRequestService = new();
            CourseService courseService = new CourseService();
            courseService.RemoveStudent(course.Id);
            withdrawalRequestService.UpdateStatus(SelectedWithdrawal.Id, Status.Accepted);
            EnrollmentRequestService enrollmentRequestService = new();
            enrollmentRequestService.ResumePausedRequests(SelectedWithdrawal.Student);
            Update();
        }

        public void RejectWithdrawal()
        {
            WithdrawalRequestService withdrawalRequestService = new();
            PenaltyPointService penaltyPointService = new();
            withdrawalRequestService.UpdateStatus(SelectedWithdrawal.Id, Status.Accepted);

            CourseService courseService = new();
            TutorService tutorService = new();
            courseService.RemoveStudent(course.Id);

            EnrollmentRequestService enrollmentRequestService = new();
            enrollmentRequestService.ResumePausedRequests(SelectedWithdrawal.Student);

            penaltyPointService.GivePenaltyPoint(SelectedStudent.ToStudent(), tutorService.Get(course.TutorId), course.ToCourse());

            NotifyStudentAboutPenaltyPoint(SelectedStudent.Id);
            Update();
        }
    }
}
