﻿using LangLang.Core.Controller;
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

        public ExamAppRequest Add(ExamAppRequest appRequest, ExamSlotController examController)
        {
            appRequest.Id = GenerateId();

            ExamSlot? exam = examController.GetById(appRequest.ExamSlotId);
            examController.AddStudent(exam);

            _appRequests.Add(appRequest.Id, appRequest);
            _repository.Save(_appRequests);
            NotifyObservers();
            return appRequest;
        }


        public ExamAppRequest? Remove(int id, ExamSlotController examController)
        {
            ExamAppRequest? appRequest = GetAppRequestById(id);
            if (appRequest == null) return null;

            ExamSlot? exam = examController.GetById(appRequest.ExamSlotId);
            examController.RemoveStudent(exam);

            _appRequests.Remove(appRequest.Id);
            _repository.Save(_appRequests);
            NotifyObservers();
            return appRequest;
        }
        //returns list of all students application requests for exams
        public List<ExamAppRequest> GetStudentRequests(int studentId)
        {
            List<ExamAppRequest> studentRequests = new();
            foreach (ExamAppRequest appRequest in GetAllAppRequests())
            {
                if (appRequest.StudentId == studentId) studentRequests.Add(appRequest);
            }
            return studentRequests;
        }
        //returns list of all students application requests for exams (without canceled ones and ones that passed)
        public List<ExamAppRequest> GetActiveStudentRequests(int studentId, ExamSlotController examSlotController)
        {
            List<ExamAppRequest> studentRequests = new();
            foreach (ExamAppRequest appRequest in GetAllAppRequests())
            {
                if (appRequest.StudentId == studentId && !appRequest.IsCanceled && IsRequestActive(appRequest, examSlotController)) studentRequests.Add(appRequest);
            }
            return studentRequests;
        }

        //checks  if the exam slot associated with the request has already passed
        public bool IsRequestActive(ExamAppRequest request , ExamSlotController examSlotController)
        {
            ExamSlot exam = examSlotController.GetById(request.ExamSlotId);
            return !examSlotController.HasPassed(exam);

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
            appRequest.CancelExamAppRequest();
            return true;
        }

    }
}
