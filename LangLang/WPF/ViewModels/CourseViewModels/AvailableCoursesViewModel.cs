using LangLang.Aplication.UseCases;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using LangLang.Domain.Models;
using LangLang.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModel
{
    public class AvailableCoursesViewModel
    {
        public ObservableCollection<CourseDTO> Courses { get; set; }
        public List<Course> CoursesForReview { get; set; }
        private EnrollmentRequestDTO EnrollmentRequest { get; set; }
        public CourseDTO SelectedCourse { get; set; }
        private Student currentlyLoggedIn { get; set; }

        public AvailableCoursesViewModel(Student currentlyLoggedIn)
        {
            this.currentlyLoggedIn = currentlyLoggedIn;
            Courses = new();
            EnrollmentRequest = new();
        }

        public void SetDataForReview()
        {
            var courseService = new CourseController();
            CoursesForReview = courseService.GetAvailable(currentlyLoggedIn);
        }

        public void Search(string language, LanguageLevel? level, DateTime courseStartDate, int duration, bool? online)
        {
            var courseService = new CourseController();
            CoursesForReview = courseService.SearchCoursesByStudent(currentlyLoggedIn, language, level, courseStartDate, duration, online);
        }

        public void Clear()
        {
            var courseService = new CourseController();
            CoursesForReview = courseService.GetAvailable(currentlyLoggedIn);
        }

        public void SendRequest()
        {
            var studentService = new StudentService();
            bool canApplyForCourses = studentService.CanApplyForCourses(currentlyLoggedIn);
            if (!canApplyForCourses)
            {
                MessageBox.Show("Can't apply for the course as all not all your exams have been graded.");
            }

            if (SelectedCourse == null) return;

            EnrollmentRequest.CourseId = SelectedCourse.Id;
            EnrollmentRequest.StudentId = currentlyLoggedIn.Id;
            EnrollmentRequest.Status = Status.Pending;
            EnrollmentRequest.RequestSentAt = DateTime.Now;
            EnrollmentRequest.LastModifiedAt = DateTime.Now;
            EnrollmentRequest.IsCanceled = false;

            var enrollmentService = new EnrollmentRequestService();
            enrollmentService.Add(EnrollmentRequest.ToEnrollmentRequest());

            MessageBox.Show("Request sent. Please wait for approval.");

            var courseService = new CourseController();
            CoursesForReview = courseService.GetAvailable(currentlyLoggedIn);
        }

        public bool CanRequestEnroll()
        {
            var studentService = new StudentService();
            return studentService.CanRequestEnrollment(currentlyLoggedIn);
        }

        public void Update()
        {
            Courses.Clear();
            foreach (var course in CoursesForReview)
                Courses.Add(new CourseDTO(course));
        }

    }
}
