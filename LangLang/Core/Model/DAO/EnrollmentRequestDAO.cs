using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using LangLang.Core.Observer;
using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Model.DAO
{
    public class EnrollmentRequestDAO : Subject
    {
        private readonly Dictionary<int, EnrollmentRequest> _enrollmentRequests;
        private readonly Repository<EnrollmentRequest> _repository;

        public EnrollmentRequestDAO()
        {
            _repository = new Repository<EnrollmentRequest>("enrollmentRequests.csv");
            _enrollmentRequests = _repository.Load();
        }
        private int GenerateId()
        {
            if (_enrollmentRequests.Count == 0) return 0;
            return _enrollmentRequests.Keys.Max() + 1;
        }

        public EnrollmentRequest? GetEnrollmentRequestById(int id)
        {
            return _enrollmentRequests[id];
        }

        public List<EnrollmentRequest> GetAllEnrollmentRequests()
        {
            return _enrollmentRequests.Values.ToList();
        }

        public EnrollmentRequest GetById(int id)
        {
            return _enrollmentRequests[id];
        }

        public List<EnrollmentRequest> GetEnrollments(int courseId)
        {
            List<EnrollmentRequest> enrollments = new();
            foreach (EnrollmentRequest enrollment in GetAllEnrollmentRequests())
            {
                if (enrollment.CourseId == courseId)
                {
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }
        public EnrollmentRequest Add(EnrollmentRequest enrollmentRequest)
        {
            enrollmentRequest.Id = GenerateId();
            _enrollmentRequests.Add(enrollmentRequest.Id, enrollmentRequest);
            _repository.Save(_enrollmentRequests);
            NotifyObservers();
            return enrollmentRequest;
        }

        public EnrollmentRequest? Update(EnrollmentRequest enrollmentRequest)
        {
            EnrollmentRequest? oldRequest = GetEnrollmentRequestById(enrollmentRequest.Id);
            if (oldRequest == null) return null;

            oldRequest.UpdateStatus(enrollmentRequest.Status);
            _repository.Save(_enrollmentRequests);
            NotifyObservers();
            return oldRequest;
        }



        public EnrollmentRequest? Remove(int id)
        {
            EnrollmentRequest? enrollmentRequest = GetEnrollmentRequestById(id);
            if (enrollmentRequest == null) return null; 

            _enrollmentRequests.Remove(enrollmentRequest.Id);
            _repository.Save(_enrollmentRequests);
            NotifyObservers();
            return enrollmentRequest;
        }

        public List<EnrollmentRequest> GetStudentRequests(int studentId)
        {
            List<EnrollmentRequest> studentRequests = new();
            foreach (EnrollmentRequest enrollmentRequest in GetAllEnrollmentRequests())
            {
                if (enrollmentRequest.StudentId == studentId) studentRequests.Add(enrollmentRequest);
            }
            return studentRequests;
        }

        // returns true if the cancellation was successful, otherwise false
        public bool CancelRequest(int id, Course course)
        {
            if (course.DaysUntilStart() < Constants.COURSE_CANCELLATION_PERIOD)
                throw new Exception("Cancellation deadline passed.");

            EnrollmentRequest request = _enrollmentRequests[id];
            if (request.IsCanceled)
                throw new Exception("Already canceled.");

            request.CancelRequest();
            _repository.Save(_enrollmentRequests);
            return true;
        }

        public void PauseRequests(int studentId, int acceptedRequestId)
        {
            var studentRequests = GetStudentRequests(studentId);
            foreach (EnrollmentRequest er in studentRequests)
            {
                if (er.Id == acceptedRequestId)
                {
                    er.UpdateStatus(Status.Accepted);
                }
                else if (er.Status == Status.Pending && !er.IsCanceled)
                {
                    er.UpdateStatus(Status.Paused);
                }
            }
        }

        public void ResumePausedRequests(int studentId)
        {
            var studentRequests = GetStudentRequests(studentId);
            foreach (EnrollmentRequest request in studentRequests)
            {
                if (request.Status == Status.Paused) request.UpdateStatus(Status.Pending);
            }
        }

        public EnrollmentRequest? GetActiveCourseRequest(int studentId, AppController appController)
        {
            var courseController = appController.CourseController;
            var withdrawalController = appController.WithdrawalRequestController;
            var studentRequests = GetStudentRequests(studentId);

            foreach (var request in studentRequests)
            {
                var course = courseController.GetById(request.CourseId);

                if (IsCurrentCourseRequest(request, course, withdrawalController))
                    return request;
            }

            return null;
        }

        private bool IsCurrentCourseRequest(EnrollmentRequest request, Course course, WithdrawalRequestController wrController)
        {
            if (request.Status != Status.Accepted || course.IsCompleted())
            {
                return false;
            }

            return !wrController.HasAcceptedWithdrawal(request.Id);
        }

        public bool CanRequestWithdrawal(int id)
        {
            EnrollmentRequest er = GetById(id);
            return er.CanWithdraw();
        }

        public bool IsRequestDuplicate(int studentId, Course course)
        {
            foreach (EnrollmentRequest er in GetAllEnrollmentRequests())
            {
                if (er.StudentId == studentId && er.CourseId == course.Id && !er.IsCanceled) return true;
            }
            return false;
        }
    }
}
