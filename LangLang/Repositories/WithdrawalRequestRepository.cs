using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Configuration;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangLang.Repositories
{
    public class WithdrawalRequestRepository : Subject, IWithdrawalRequestRepository
    {
        private Dictionary<int, WithdrawalRequest> _withdrawalRequests;
        private const string _filePath = Constants.FILENAME_PREFIX + "withdrawalRequests.csv";

        public WithdrawalRequestRepository() {
            _withdrawalRequests = Load();
        }

        public WithdrawalRequest Get(int id)
        {
            return _withdrawalRequests[id];
        }

        public List<WithdrawalRequest> GetAll()
        {
            return _withdrawalRequests.Values.ToList();
        }


        public void Add(WithdrawalRequest request)
        {
            _withdrawalRequests.Add(request.Id, request);
            Save();
            NotifyObservers();
        }

        public void Delete(int id)
        {
            WithdrawalRequest? request = Get(id);
            if (request == null) return;

            _withdrawalRequests.Remove(request.Id);
            Save();
            NotifyObservers();
        }

        public void Update(WithdrawalRequest request)
        {
            WithdrawalRequest? oldRequest = Get(request.Id);
            if (oldRequest == null) return;

            oldRequest.UpdateStatus(request.Status);
            Save();
            NotifyObservers();
        }

        public List<WithdrawalRequest> GetByStudent(Student student)
        {
            List<WithdrawalRequest> studentRequests = new();
            var enrollmentReqService = new EnrollmentRequestService();
            List<EnrollmentRequest> allEnrollmentRequests = enrollmentReqService.GetAll();

            foreach (WithdrawalRequest request in GetAll())
            {
                EnrollmentRequest enrollmentRequest = allEnrollmentRequests[request.EnrollmentRequestId];
                if (enrollmentRequest.StudentId == student.Id)
                {
                    studentRequests.Add(request);
                }
            }
            return studentRequests;
        }

        public List<WithdrawalRequest> GetByCourse(Course course)
        {
            List<WithdrawalRequest> courseRequests = new();
            var enrollmentReqService = new EnrollmentRequestService();
            List<EnrollmentRequest> allEnrollmentRequests = enrollmentReqService.GetAll();
            
            foreach (WithdrawalRequest request in GetAll())
            {
                EnrollmentRequest enrollmentRequest = allEnrollmentRequests[request.EnrollmentRequestId];
                if (enrollmentRequest.CourseId == course.Id)
                {
                    courseRequests.Add(request);
                }
            }
            return courseRequests;
        }

        public void Save()
        {
            var lines = GetAll().Select(wr =>
            {
                return string.Join(Constants.DELIMITER,
                    wr.Id.ToString(),
                    wr.EnrollmentRequestId.ToString(),
                    wr.Status.ToString(),
                    wr.RequestSentAt.ToString(Constants.DATE_FORMAT),
                    wr.RequestReceivedAt.ToString(Constants.DATE_FORMAT),
                    wr.Reason);
            });

            File.WriteAllLines(_filePath, lines);
        }
        public Dictionary<int, WithdrawalRequest> Load()
        {
            Dictionary<int, WithdrawalRequest> withdrawalRequests = new Dictionary<int, WithdrawalRequest>();

            if (!File.Exists(_filePath)) return withdrawalRequests;

            
            string[] lines = File.ReadAllLines(_filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(Constants.DELIMITER);

                int id = int.Parse(parts[0]);
                int enrollmentRequestId = int.Parse(parts[1]);
                Status status = Enum.Parse<Status>(parts[2]);
                DateTime requestSentAt;
                DateTime requestReceivedAt;
                try
                {
                    requestSentAt = DateTime.ParseExact(parts[3], Constants.DATE_FORMAT, null);
                    requestReceivedAt = DateTime.ParseExact(parts[4], Constants.DATE_FORMAT, null);

                } catch (Exception ex)
                {
                    throw new FormatException("Date is not in the correct format.");
                }
                string reason = parts[5];

                WithdrawalRequest withdrawalRequest = new WithdrawalRequest(id, enrollmentRequestId, reason, status, requestSentAt, requestReceivedAt);

                withdrawalRequests.Add(withdrawalRequest.Id, withdrawalRequest);
                    
            }

            return withdrawalRequests;
        }

    }
}
