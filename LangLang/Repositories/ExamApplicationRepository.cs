using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangLang.Repositories
{
    public class ExamApplicationRepository : IExamApplicationRepository
    {
        private Dictionary<int, ExamApplication> _applications;
        private const string _filePath = Constants.FILENAME_PREFIX + "examApplications.csv";
        public ExamApplicationRepository()
        {
            _applications = Load();
        }

        public ExamApplication Get(int id)
        {
            return _applications[id];
        }

        public List<ExamApplication> GetAll()
        {
            return _applications.Values.ToList();
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
        public void Add(ExamApplication app)
        {
            _applications.Add(app.Id, app);
            Save();
        }

        public void Delete(int id)
        {
            _applications.Remove(id);
            Save();
        }

        public Dictionary<int, ExamApplication> Load()
        {
            var applications = new Dictionary<int, ExamApplication>();

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var values = line.Split(Constants.DELIMITER);

                int id = int.Parse(values[0]);
                int studentId = int.Parse(values[1]);
                int examSlotId = int.Parse(values[2]);
                DateTime sentAt;
                try
                {
                    sentAt = DateTime.ParseExact(values[3], Constants.DATE_FORMAT, null);
                }
                catch
                {
                    throw new FormatException("Date is not in the correct format.");
                }

                var application = new ExamApplication(id, studentId, examSlotId, sentAt);
                applications.Add(application.Id, application);
            }
            return applications;
        }

        public void Save()
        {
            var lines = GetAll().Select(app =>
            {
                return string.Join(Constants.DELIMITER,
                app.Id.ToString(),
                app.StudentId.ToString(),
                app.ExamSlotId.ToString(),
                app.SentAt.ToString(Constants.DATE_FORMAT));
            });

            File.WriteAllLines(_filePath, lines);
        }
    }
}
