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
        public bool CancelRequest(EnrollmentRequest enrollmentRequest, Course course)
        {
            if (course.StartDateTime.Date - DateTime.Now.Date <= TimeSpan.FromDays(7))
                return false; // course start date must be at least 7 days away

            enrollmentRequest.CancelRequest();
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

        public EnrollmentRequest? GetActiveCourseRequest(int studentId, CourseController courseController, WithdrawalRequestController wrController)
        {
            var studentRequests = GetStudentRequests(studentId);

            foreach (var request in studentRequests)
            {
                var course = courseController.GetById(request.CourseId);

                if (IsCurrentCourseRequest(request, course, wrController))
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

    }
}
