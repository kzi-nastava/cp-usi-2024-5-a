using LangLang.Core.Model.Enums;
using LangLang.Core.Observer;
using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Model.DAO
{
    public class ExamSlotAppRequestDAO : Subject
    {
        private readonly Dictionary<int, ExamSlotAppRequest> _appRequests;
        private readonly Repository<ExamSlotAppRequest> _repository;

        public ExamSlotAppRequestDAO()
        {
            _repository = new Repository<ExamSlotAppRequest>("examSlotAppRequests.csv");
            _appRequests = _repository.Load();
        }
        private int GenerateId()
        {
            if (_appRequests.Count == 0) return 0;
            return _appRequests.Keys.Max() + 1;
        }

        public ExamSlotAppRequest? GetAppRequestById(int id)
        {
            return _appRequests[id];
        }

        public List<ExamSlotAppRequest> GetAllAppRequests()
        {
            return _appRequests.Values.ToList();
        }

        public ExamSlotAppRequest Add(ExamSlotAppRequest appRequest)
        {
            appRequest.Id = GenerateId();
            _appRequests.Add(appRequest.Id, appRequest);
            _repository.Save(_appRequests);
            NotifyObservers();
            return appRequest;
        }


        public ExamSlotAppRequest? Remove(int id)
        {
            ExamSlotAppRequest? appRequest = GetAppRequestById(id);
            if (appRequest == null) return null;

            _appRequests.Remove(appRequest.Id);
            _repository.Save(_appRequests);
            NotifyObservers();
            return appRequest;
        }

        public List<ExamSlotAppRequest> GetStudentRequests(int studentId)
        {
            List<ExamSlotAppRequest> studentRequests = new();
            foreach (ExamSlotAppRequest appRequest in GetAllAppRequests())
            {
                if (appRequest.StudentId == studentId) studentRequests.Add(appRequest);
            }
            return studentRequests;
        }

        // returns true if the cancellation was successful, otherwise false
        public bool CancelRequest(ExamSlotAppRequest appRequest, ExamSlot exam)
        {
            if (exam.TimeSlot.Time.Date - DateTime.Now.Date <= TimeSpan.FromDays(10))
                return false; // exam start date must be at least 10 days away
            appRequest.CancelExamSlot();
            return true;
        }

    }
}
