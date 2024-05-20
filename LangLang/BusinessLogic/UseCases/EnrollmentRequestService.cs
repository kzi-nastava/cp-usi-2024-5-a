
using LangLang.Core.Model.Enums;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System;
using LangLang.Domain.RepositoryInterfaces;
using System.Linq;
using LangLang.Core.Model;
using LangLang.Core;
using LangLang.Core.Controller;
using LangLang.Core.Observer;
using LangLang.Composition;

namespace LangLang.BusinessLogic.UseCases
{
    public class EnrollmentRequestService
    {
        private IEnrollmentRequestRepository _enrollmentRequests;
        
        public EnrollmentRequestService() {
            _enrollmentRequests = Injector.CreateInstance<IEnrollmentRequestRepository>();
        }

        private int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public EnrollmentRequest Get(int id)
        {
            return _enrollmentRequests.Get(id);
        }

        public List<EnrollmentRequest> GetAll()
        {
            return _enrollmentRequests.GetAll();
        }

        public void Add(EnrollmentRequest enrollmentRequest)
        {
            enrollmentRequest.Id = GenerateId();
            _enrollmentRequests.Add(enrollmentRequest);
        }

        public void Delete(int id)
        {
            _enrollmentRequests.Delete(id);
        }

        public void Update(EnrollmentRequest enrollmentRequest)
        {
            _enrollmentRequests.Update(enrollmentRequest);
        }

        public Dictionary<int, EnrollmentRequest> Load()
        {
           return _enrollmentRequests.Load();
        }

        public void Save()
        {
            _enrollmentRequests.Save();
        }

        public List<EnrollmentRequest> GetByStudent(Student student)
        {
            return _enrollmentRequests.GetByStudent(student);
        }

        public List<EnrollmentRequest> GetByCourse(Course course)
        {
           return _enrollmentRequests.GetByCourse(course);
        }

        public void CancelRequest(int id, Course course)
        {
            if (course.DaysUntilStart() < Constants.COURSE_CANCELLATION_PERIOD)
                throw new Exception("Cancellation deadline passed.");

            _enrollmentRequests.Cancel(id);
        }

        public void Cancel(EnrollmentRequest request)
        {
            _enrollmentRequests.Cancel(request.Id);
        }

        public void PauseRequests(Student student, int acceptedRequestId)
        {
            var studentRequests = GetByStudent(student);
            foreach (EnrollmentRequest er in studentRequests)
            {
                if (er.Id == acceptedRequestId)
                {
                    _enrollmentRequests.Accept(er);
                }
                else if (er.Status == Status.Pending && !er.IsCanceled)
                {
                    _enrollmentRequests.Pause(er);
                }
            }
        }
        public void ResumePausedRequests(Student student)
        {
            var studentRequests = GetByStudent(student);
            foreach (EnrollmentRequest request in studentRequests)
            {
                if (request.Status == Status.Paused) _enrollmentRequests.SetPending(request);
            }
        }

        public EnrollmentRequest? GetActiveCourseRequest(Student student)
        {
            var courseService = new CourseController();
            var studentRequests = GetByStudent(student);

            foreach (var request in studentRequests)
            {
                var course = courseService.Get(request.CourseId);

                if (IsCurrentCourseRequest(request, course))
                    return request;
            }

            return null;
        }

        private bool IsCurrentCourseRequest(EnrollmentRequest request, Course course)
        {
            if (request.Status != Status.Accepted || course.IsCompleted())
            {
                return false;
            }

            var withdrawalService = new WithdrawalRequestController();
            return !withdrawalService.HasAcceptedWithdrawal(request.Id);
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

        public void Subscribe(IObserver observer)
        {
            _enrollmentRequests.Subscribe(observer);
        }
    }
}
