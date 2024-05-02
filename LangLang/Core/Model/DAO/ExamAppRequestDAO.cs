using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using LangLang.Core.Observer;
using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Model.DAO
{
    public class ExamAppRequestDAO : Subject
    {
        private readonly Dictionary<int, ExamAppRequest> _appRequests;
        private readonly Repository<ExamAppRequest> _repository;

        public ExamAppRequestDAO()
        {
            _repository = new Repository<ExamAppRequest>("examSlotAppRequests.csv");
            _appRequests = _repository.Load();
        }
        private int GenerateId()
        {
            if (_appRequests.Count == 0) return 0;
            return _appRequests.Keys.Max() + 1;
        }

        public ExamAppRequest? GetAppRequestById(int id)
        {
            return _appRequests[id];
        }

        public List<ExamAppRequest> GetAllAppRequests()
        {
            return _appRequests.Values.ToList();
        }

        public ExamAppRequest Add(ExamAppRequest appRequest)
        {
            appRequest.Id = GenerateId();
            _appRequests.Add(appRequest.Id, appRequest);
            _repository.Save(_appRequests);
            NotifyObservers();
            return appRequest;
        }


        public ExamAppRequest? Remove(int id)
        {
            ExamAppRequest? appRequest = GetAppRequestById(id);
            if (appRequest == null) return null;

            _appRequests.Remove(appRequest.Id);
            _repository.Save(_appRequests);
            NotifyObservers();
            return appRequest;
        }

        public List<ExamAppRequest> GetStudentRequests(int studentId)
        {
            List<ExamAppRequest> studentRequests = new();
            foreach (ExamAppRequest appRequest in GetAllAppRequests())
            {
                if (appRequest.StudentId == studentId) studentRequests.Add(appRequest);
            }
            return studentRequests;
        }

        public List<Student> GetExamRequests(int examId, StudentController studentController)
        {
            List<Student> students = new();
            foreach (ExamAppRequest appRequest in GetAllAppRequests())
            {
                if (appRequest.ExamSlotId == examId) {
                    Student student = studentController.GetById(appRequest.StudentId);
                    students.Add(student);
                }
            }
            return students;
        }

        // returns true if the cancellation was successful, otherwise false
        public bool CancelRequest(ExamAppRequest appRequest, ExamSlot exam)
        {
            if (exam.TimeSlot.Time.Date - DateTime.Now.Date <= TimeSpan.FromDays(10))
                return false; // exam start date must be at least 10 days away
            appRequest.CancelExamSlot();
            return true;
        }

    }
}
