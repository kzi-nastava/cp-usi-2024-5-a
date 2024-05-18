using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using LangLang.Core.Observer;
using LangLang.Core.Repository;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Model.DAO
{
    public class ExamApplicationDAO : Subject
    {
        private readonly Dictionary<int, ExamApplication> _applications;
        private readonly Repository<ExamApplication> _repository;

        public ExamApplicationDAO()
        {
            _repository = new Repository<ExamApplication>("examApplications.csv");
            _applications = _repository.Load();
        }
        private int GenerateId()
        {
            if (_applications.Count == 0) return 0;
            return _applications.Keys.Max() + 1;
        }

        public ExamApplication? Get(int id)
        {
            return _applications[id];
        }

        public List<ExamApplication> GetAll()
        {
            return _applications.Values.ToList();
        }

        public ExamApplication Add(ExamApplication application, ExamSlotController examController)
        {
            application.Id = GenerateId();

            ExamSlot? exam = examController.Get(application.ExamSlotId);
            examController.AddStudent(exam);

            _applications.Add(application.Id, application);
            _repository.Save(_applications);
            NotifyObservers();
            return application;
        }


        public ExamApplication? Remove(int id, ExamSlotController examController)
        {
            ExamApplication? application = Get(id);
            if (application == null) return null;

            ExamSlot? exam = examController.Get(application.ExamSlotId);
            examController.RemoveStudent(exam);

            _applications.Remove(application.Id);
            _repository.Save(_applications);
            NotifyObservers();
            return application;
        }
        //returns list of all students applications for exams
        public List<ExamApplication> GetApplications(Student student)
        {
            List<ExamApplication> studentApplications = new();
            foreach (ExamApplication app in GetAll())
            {
                if (app.StudentId == student.Id) studentApplications.Add(app);
            }
            return studentApplications;
        }
        //returns list of all students applications for exams (without ones that passed)
        public List<ExamApplication> GetActiveStudentApplications(int studentId)
        {
            var examSlotService = new ExamSlotController();
            List<ExamApplication> studentApplications = new();
            foreach (ExamApplication app in GetAll())
            {
                if (app.StudentId == studentId && IsApplicationActive(app, examSlotService)) studentApplications.Add(app);
            }
            return studentApplications;
        }

        //checks  if the exam slot associated with the application has already passed
        public bool IsApplicationActive(ExamApplication application , ExamSlotController examSlotController)
        {
            ExamSlot exam = examSlotController.Get(application.ExamSlotId);
            return !examSlotController.HasPassed(exam);

        }

        public List<ExamApplication> GetApplications(int examId)
        {
            List<ExamApplication> applications = new();
            foreach (ExamApplication application in GetAll())
            {
                if (application.ExamSlotId == examId)
                {
                    applications.Add(application);
                }
            }
            return applications;
        }

        // returns true if the cancellation was successful, otherwise false
        public bool CancelApplication(ExamApplication application, ExamSlot exam,ExamSlotController examSlotController)
        {
            if (!CanBeCanceled(exam)) 
            {
                return false; // exam start date must be at least 10 days away
            } 
            Remove(application.Id, examSlotController);
            return true;
        }
        private bool CanBeCanceled(ExamSlot exam)
        {
            return (exam.TimeSlot.Time.Date - DateTime.Now.Date) > TimeSpan.FromDays(Constants.EXAM_CANCELATION_PERIOD);
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
            var examSlotService = new ExamSlotController();
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
