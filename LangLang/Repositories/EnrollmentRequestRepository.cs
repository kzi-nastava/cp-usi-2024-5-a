using LangLang.Configuration;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LangLang.Domain.Enums;

namespace LangLang.Repositories
{
    public class EnrollmentRequestRepository : Subject, IEnrollmentRequestRepository
    {
        private Dictionary<int, EnrollmentRequest> _enrollmentRequests;
        private const string _filePath = Constants.FILENAME_PREFIX + "enrollmentRequests.csv";
        public EnrollmentRequestRepository() {
            _enrollmentRequests = Load();
        }

        public EnrollmentRequest Get(int id)
        {
            return _enrollmentRequests[id];
        }

        public List<EnrollmentRequest> GetAll()
        {
            return _enrollmentRequests.Values.ToList();
        }

        public void Add(EnrollmentRequest enrollmentRequest)
        {
            _enrollmentRequests.Add(enrollmentRequest.Id, enrollmentRequest);
            Save();
            NotifyObservers();
        }

        public void Delete(int id)
        {
            EnrollmentRequest? enrollmentRequest = Get(id);
            if (enrollmentRequest == null) return;

            _enrollmentRequests.Remove(enrollmentRequest.Id);
            Save();
            NotifyObservers();
        }

        public void Update(EnrollmentRequest enrollmentRequest)
        {
            EnrollmentRequest? oldRequest = Get(enrollmentRequest.Id);
            if (oldRequest == null) return;

            oldRequest.UpdateStatus(enrollmentRequest.Status);
            Save();
            NotifyObservers();
        }

        public List<EnrollmentRequest> GetByStudent(Student student)
        {
            List<EnrollmentRequest> studentRequests = new();
            foreach (EnrollmentRequest enrollmentRequest in GetAll())
            {
                if (enrollmentRequest.StudentId == student.Id) studentRequests.Add(enrollmentRequest);
            }
            return studentRequests;
        }

        public void Cancel(int id)
        {
            EnrollmentRequest request = Get(id);
            if (request.IsCanceled)
                throw new Exception("Already canceled.");

            request.CancelRequest();
            Save();
        }
        
        public void Pause(EnrollmentRequest request)
        {
            request.UpdateStatus(Status.Paused);
            Save();
        }

        public void Accept(EnrollmentRequest request)
        {
            request.UpdateStatus(Status.Accepted);
            Save();
        }

        public void SetPending(EnrollmentRequest request)
        {
            request.UpdateStatus(Status.Pending);
            Save();
        }

        public List<EnrollmentRequest> GetByCourse(Course course)
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

        public Dictionary<int, EnrollmentRequest> Load()
        {
            _enrollmentRequests = new();

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var values = line.Split(Constants.DELIMITER);

                int id = int.Parse(values[0]);
                int studentId = int.Parse(values[1]);
                int courseId = int.Parse(values[2]);
                Status status = (Status)Enum.Parse(typeof(Status), values[3]);
                DateTime requestSentAt;
                DateTime lastModifiedAt;
                try
                {
                    requestSentAt = DateTime.ParseExact(values[4], Constants.DATE_FORMAT, null);
                    lastModifiedAt = DateTime.ParseExact(values[5], Constants.DATE_FORMAT, null);
                }
                catch (Exception) 
                {
                    throw new FormatException("Date is not in correct format.");
                }
                bool isCanceled = bool.Parse(values[6]);

                var enrollmentRequest = new EnrollmentRequest(id, studentId, courseId, status, requestSentAt, lastModifiedAt, isCanceled);
                _enrollmentRequests[id] = enrollmentRequest;
            }
            return _enrollmentRequests;
        }

        public void Save()
        {
            var lines = GetAll().Select(er =>
            {
                return string.Join(Constants.DELIMITER,
                    er.Id.ToString(),
                    er.StudentId.ToString(),
                    er.CourseId.ToString(),
                    er.Status.ToString(),
                    er.RequestSentAt.ToString(Constants.DATE_FORMAT),
                    er.LastModifiedAt.ToString(Constants.DATE_FORMAT),
                    er.IsCanceled.ToString());
            });

            File.WriteAllLines(_filePath, lines);
        }
    }
}
