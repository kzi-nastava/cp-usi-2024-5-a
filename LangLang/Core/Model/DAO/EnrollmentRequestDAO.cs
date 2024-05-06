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

        public List<EnrollmentRequest> GetAll()
        {
            return _enrollmentRequests.Values.ToList();
        }

        public EnrollmentRequest Get(int id)
        {
            return _enrollmentRequests[id];
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
            EnrollmentRequest? oldRequest = Get(enrollmentRequest.Id);
            if (oldRequest == null) return null;

            oldRequest.UpdateStatus(enrollmentRequest.Status);
            _repository.Save(_enrollmentRequests);
            NotifyObservers();
            return oldRequest;
        }

        public EnrollmentRequest? Remove(int id)
        {
            EnrollmentRequest? enrollmentRequest = Get(id);
            if (enrollmentRequest == null) return null;

            _enrollmentRequests.Remove(enrollmentRequest.Id);
            _repository.Save(_enrollmentRequests);
            NotifyObservers();
            return enrollmentRequest;
        }

        public List<EnrollmentRequest> GetRequests(Student student)
        {
            List<EnrollmentRequest> studentRequests = new();
            foreach (EnrollmentRequest enrollmentRequest in GetAll())
            {
                if (enrollmentRequest.StudentId == student.Id) studentRequests.Add(enrollmentRequest);
            }
            return studentRequests;
        }

        public List<EnrollmentRequest> GetRequests(Course course)
        {
            List<EnrollmentRequest> enrollments = new();
            foreach (EnrollmentRequest enrollment in GetAll())
            {
                if (enrollment.CourseId == course.Id)
                {
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
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

        public void PauseRequests(Student student, int acceptedRequestId)
        {
            var studentRequests = GetRequests(student);
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

        public void ResumePausedRequests(Student student)
        {
            var studentRequests = GetRequests(student);
            foreach (EnrollmentRequest request in studentRequests)
            {
                if (request.Status == Status.Paused) request.UpdateStatus(Status.Pending);
            }
        }

        public EnrollmentRequest? GetActiveCourseRequest(Student student, AppController appController)
        {
            var courseController = appController.CourseController;
            var withdrawalController = appController.WithdrawalRequestController;
            var studentRequests = GetRequests(student);

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
            EnrollmentRequest er = Get(id);
            return er.CanWithdraw();
        }

        public bool AlreadyExists(Student student, Course course)
        {
            foreach (EnrollmentRequest er in GetAll())
            {
                if (er.StudentId == student.Id && er.CourseId == course.Id && !er.IsCanceled) return true;
            }
            return false;
        }
    }
}