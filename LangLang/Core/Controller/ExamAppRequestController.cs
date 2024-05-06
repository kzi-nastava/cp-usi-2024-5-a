﻿using LangLang.Core.Model.DAO;
using System.Collections.Generic;
using LangLang.Core.Model;
using LangLang.Core.Observer;

namespace LangLang.Core.Controller
{
    public class ExamAppRequestController
    {
        private readonly ExamAppRequestDAO _appRequests;

        public ExamAppRequestController()
        {
            _appRequests = new ExamAppRequestDAO();
        }

        public List<ExamAppRequest> GetAll()
        {
            return _appRequests.GetAllAppRequests();
        }

        public void Add(ExamAppRequest appRequest, ExamSlotController examSlotController)
        {
            _appRequests.Add(appRequest, examSlotController);
        }

        public void Delete(int id, ExamSlotController examSlotController)
        {
            _appRequests.Remove(id, examSlotController);
        }

        
        public void Subscribe(IObserver observer)
        {
            _appRequests.Subscribe(observer);
        }

        public List<ExamAppRequest> GetRequests(Student student)
        {
            return _appRequests.GetRequests(student);
        }
        public List<ExamAppRequest> GetActiveStudentRequests(int studentId, ExamSlotController examSlotController)
        {
            return _appRequests.GetActiveStudentRequests(studentId, examSlotController);
        }
        public List<ExamAppRequest> GetApplications(int examId)
        {
            return _appRequests.GetApplications(examId);
        }

        public bool CancelRequest(ExamAppRequest appRequest, ExamSlotController examSlotController)
        {
            ExamSlot exam = examSlotController.GetById(appRequest.ExamSlotId);
            return _appRequests.CancelRequest(appRequest, exam);
        }

        //Checks if student has taken exams that don't have generated results yet
        public bool HasNoGeneratedResults(Student student, ExamSlotController examSlotController)
        {
            return _appRequests.HasNoGeneratedResults(student, examSlotController);
        }

    }
}