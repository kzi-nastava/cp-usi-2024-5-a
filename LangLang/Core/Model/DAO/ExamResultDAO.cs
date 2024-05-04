using LangLang.Core.Controller;
using LangLang.Core.Model.Enums;
using LangLang.Core.Observer;
using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LangLang.Core.Model.DAO
{
    public class ExamResultDAO : Subject
    {
        private readonly Dictionary<int, ExamResult> _examResults;
        private readonly Repository<ExamResult> _repository;


        public ExamResultDAO()
        {
            _repository = new Repository<ExamResult>("examresults.csv");
            _examResults = _repository.Load();
        }

        private int GenerateId()
        {
            if (_examResults.Count == 0) return 0;
            return _examResults.Keys.Max() + 1;
        }

        public ExamResult GetById(int id)
        {
            return _examResults[id];
        }

        public List<ExamResult> GetAll()
        {
            return _examResults.Values.ToList();
        }

        // NOTE: IT'S NOT FINAL VERSION - will be updated when ExamApplicationDTO is implemented
        public void Add(int studentId, int examId)
        {
            ExamResult examResult = new();

            examResult.Id = GenerateId();
            examResult.StudentId = studentId;
            examResult.ExamSlotId = examId;

            _examResults.Add(examResult.Id , examResult);
            _repository.Save(_examResults);
            NotifyObservers();
        }

        public ExamResult Update(ExamResult examResult)
        {
            ExamResult oldResult = GetById(examResult.Id);
            if (oldResult == null) return null;

            oldResult.ReadingPoints = examResult.ReadingPoints; 
            oldResult.ListeningPoints = examResult.ListeningPoints;
            oldResult.WritingPoints = examResult.WritingPoints;
            oldResult.SpeakingPoints = examResult.SpeakingPoints;
            oldResult.EvaluateOutcome();

            _repository.Save(_examResults);
            NotifyObservers();
            return oldResult;
        }

        public List<ExamResult> Get(int examId) {
            List<ExamResult> results = new();
            foreach (ExamResult examResult in GetAll())
            {
                if (examResult.ExamSlotId == examId) results.Add(examResult);
            }
            return results;
        }

        // TODO: change after implementing the examApplicationDTO
        public void GenerateResults(List<Student>students, int examId)
        {
            foreach (Student student in students)
            {
                Add(student.Id, examId);
            }
        }

    }
}
