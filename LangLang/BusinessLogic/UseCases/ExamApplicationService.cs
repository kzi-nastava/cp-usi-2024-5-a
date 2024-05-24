using LangLang.Composition;
using LangLang.Core;
using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using LangLang.Core.Observer;
using LangLang.Core.Repository;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.BusinessLogic.UseCases
{
    public class ExamApplicationService : Subject
    {
        private IExamApplicationRepository _applications;

        public ExamApplicationService()
        {
            _applications = Injector.CreateInstance<IExamApplicationRepository>();
        }

        private int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public ExamApplication? Get(int id)
        {
            return _applications.Get(id);
        }

        public List<ExamApplication> GetAll()
        {
            return _applications.GetAll();
        }

        public ExamApplication Add(ExamApplication application)
        {
            application.Id = GenerateId();

            var examService = new ExamSlotService();
            ExamSlot? exam = examService.Get(application.ExamSlotId);
            examService.AddStudent(exam);
            _applications.Add(application);
            return application;
        }


        public ExamApplication? Delete(int id)
        {
            ExamApplication? application = Get(id);
            if (application == null) return null;

            var examService = new ExamSlotService();
            ExamSlot? exam = examService.Get(application.ExamSlotId);
            examService.RemoveStudent(exam);
            _applications.Delete(application.Id);
            return application;
        }
        public List<ExamApplication> GetApplications(Student student)
        {
            return _applications.GetApplications(student);
        }

        public List<ExamApplication> GetApplications(int examId)
        {
            return GetApplications(examId);
        }

        //returns list of all students applications for exams (without ones that passed)
        public List<ExamApplication> GetActiveStudentApplications(int studentId)
        {
            List<ExamApplication> studentApplications = new();
            foreach (ExamApplication app in GetAll())
            {
                if (app.StudentId == studentId && IsApplicationActive(app)) studentApplications.Add(app);
            }
            return studentApplications;
        }

        //checks  if the exam slot associated with the application has already passed
        public bool IsApplicationActive(ExamApplication application)
        {
            var examService = new ExamSlotService();
            ExamSlot exam = examService.Get(application.ExamSlotId);
            return !examService.HasPassed(exam);
        }

        
        // returns true if the cancellation was successful, otherwise false
        public bool CancelApplication(ExamApplication application)
        {
            ExamSlotService examService = new();
            ExamSlot exam = examService.Get(application.ExamSlotId);
            if (!CanBeCanceled(exam))
            {
                return false; // exam start date must be at least 10 days away
            }
            Delete(application.Id);
            return true;
        }
        private bool CanBeCanceled(ExamSlot exam)
        {
            return exam.TimeSlot.Time.Date - DateTime.Now.Date > TimeSpan.FromDays(Constants.EXAM_CANCELATION_PERIOD);
        }

        public bool HasApplied(Student student, ExamSlot exam)
        {
            List<ExamApplication> studentApplications = GetApplications(student);
            foreach (ExamApplication app in studentApplications)
            {
                if (app.ExamSlotId == exam.Id) return true;
            }
            return false;
        }

        public bool HasNoGeneratedResults(Student student)
        {
            var examSlotService = new ExamSlotService();
            foreach (ExamApplication application in GetApplications(student))
            {
                ExamSlot exam = examSlotService.Get(application.ExamSlotId);
                bool hasPassed = examSlotService.HasPassed(exam);

                if (hasPassed && !exam.ResultsGenerated)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
