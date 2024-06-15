using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using LangLang.Domain.Models;
using System.Linq;
using System.IO;
using LangLang.Configuration;
using LangLang.Domain.Enums;

namespace LangLang.Repositories.FileRepositories
{
    public class ExamResultRepository : IExamResultRepository
    {

        private readonly Dictionary<int, ExamResult> _examResults;
        private const string _filePath = Constants.FILENAME_PREFIX + "examresults.csv";

        public ExamResultRepository()
        {
            _examResults = Load();
        }

        public ExamResult Get(int id)
        {
            return _examResults[id];
        }

        public List<ExamResult> GetAll()
        {
            return _examResults.Values.ToList();
        }

        public List<ExamResult> GetByExam(ExamSlot exam)
        {
            return GetAll().Where(examResult => examResult.ExamSlotId == exam.Id).ToList();
        }

        public List<ExamResult> GetByStudent(Student student)
        {
            return GetAll().Where(examResult => examResult.StudentId == student.Id).ToList();
        }

        public void Add(ExamResult examResult)
        {
            _examResults.Add(examResult.Id, examResult);
            Save();
        }

        public void Update(ExamResult examResult)
        {
            ExamResult oldResult = Get(examResult.Id);
            if (oldResult == null) return;

            oldResult.ReadingPoints = examResult.ReadingPoints;
            oldResult.ListeningPoints = examResult.ListeningPoints;
            oldResult.WritingPoints = examResult.WritingPoints;
            oldResult.SpeakingPoints = examResult.SpeakingPoints;
            oldResult.EvaluateOutcome();

            Save();
        }

        public bool HasNotGradedResults(Student student)
        {
            return GetByStudent(student).Any(examResult => examResult.Outcome == ExamOutcome.NotGraded);
        }

        public bool HasPreliminaryResults(Student student)
        {
            return GetByStudent(student).Any(examResult => examResult.Status == ResultStatus.Preliminary);
        }

        public void Save()
        {
            using (var writer = new StreamWriter(_filePath))
            {
                foreach (var result in GetAll())
                {
                    var line = string.Join(Constants.DELIMITER.ToString(),
                       result.Id,
                       result.StudentId,
                       result.ExamSlotId,
                       result.ReadingPoints,
                       result.SpeakingPoints,
                       result.ListeningPoints,
                       result.WritingPoints,
                       result.Outcome,
                       result.Status);
                    writer.WriteLine(line);
                }
            }
        }

        public Dictionary<int, ExamResult> Load()
        {
            var examResults = new Dictionary<int, ExamResult>();

            if (!File.Exists(_filePath)) return examResults;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var tokens = line.Split(Constants.DELIMITER);

                var examResult = new ExamResult
                {
                    Id = int.Parse(tokens[0]),
                    StudentId = int.Parse(tokens[1]),
                    ExamSlotId = int.Parse(tokens[2]),
                    ReadingPoints = int.Parse(tokens[3]),
                    SpeakingPoints = int.Parse(tokens[4]),
                    ListeningPoints = int.Parse(tokens[5]),
                    WritingPoints = int.Parse(tokens[6]),
                    Outcome = Enum.Parse<ExamOutcome>(tokens[7]),
                    Status = Enum.Parse<ResultStatus>(tokens[8]),
                };

                examResults.Add(examResult.Id, examResult);
            }

            return examResults;
        }

    }
}
