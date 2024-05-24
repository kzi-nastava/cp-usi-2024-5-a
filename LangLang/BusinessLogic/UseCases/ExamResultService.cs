using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Model;

namespace LangLang.BusinessLogic.UseCases
{
    public class ExamResultService
    {
        private IExamResultRepository _examResults;

        public ExamResultService()
        {
            _examResults = Injector.CreateInstance<IExamResultRepository>();
        }

        private int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        private List<ExamResult> GetAll()
        {
            return _examResults.GetAll();
        }

        public List<ExamResult> GetByExam(ExamSlot exam)
        {
            return _examResults.GetByExam(exam);
        }

        public void Add(int studentId, int examId)
        {
            ExamResult examResult = new(studentId, examId);
            examResult.Id = GenerateId();
            _examResults.Add(examResult);
        }

        public void Update(ExamResult examResult)
        {
            _examResults.Update(examResult);
        }

        public bool HasNotGradedResults(Student student)
        {
            return _examResults.HasNotGradedResults(student);
        }

        public bool HasPreliminaryResults(Student student)
        {
            return _examResults.HasPreliminaryResults(student);
        }
    }
}
