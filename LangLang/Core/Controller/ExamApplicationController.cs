﻿using System.Collections.Generic;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using LangLang.BusinessLogic.UseCases;

namespace LangLang.Core.Controller
{
    public class ExamApplicationController
    {
        private readonly ExamApplicationService _applications;

        public ExamApplicationController()
        {
            _applications = new ExamApplicationService();
        }

        public List<ExamApplication> GetAll()
        {
            return _applications.GetAll();
        }

        public void Add(ExamApplication application, ExamSlotController examSlotController)
        {
            _applications.Add(application, examSlotController);
        }

        public void Delete(int id, ExamSlotController examSlotController)
        {
            _applications.Remove(id, examSlotController);
        }

        
        public void Subscribe(IObserver observer)
        {
            _applications.Subscribe(observer);
        }

        public List<ExamApplication> GetApplications(Student student)
        {
            return _applications.GetApplications(student);
        }
        public List<ExamApplication> GetActiveStudentApplications(int studentId)
        {
            return _applications.GetActiveStudentApplications(studentId);
        }
        public List<ExamApplication> GetApplications(int examId)
        {
            return _applications.GetApplications(examId);
        }
        //Checks if student has applied for exam
        public bool HasApplied(Student student, ExamSlot exam)
        {
            return _applications.HasApplied(student, exam);
        }
        public bool CancelApplication(ExamApplication application, ExamSlotController examSlotController)
        {
            ExamSlot exam = examSlotController.Get(application.ExamSlotId);
            return _applications.CancelApplication(application, exam,examSlotController);
        }

        //Checks if student has taken exams that don't have generated results yet
        public bool HasNoGeneratedResults(Student student)
        {
            return _applications.HasNoGeneratedResults(student);
        }

    }
}