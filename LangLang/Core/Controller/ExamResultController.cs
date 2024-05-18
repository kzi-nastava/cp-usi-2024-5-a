using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Model.Enums;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Controller
{
    public class ExamResultController
    {
        private readonly ExamResultDAO _examResults;

        public ExamResultController() {
            _examResults = new ();
        }

        public void Add(int studentId, int examId)
        {
            _examResults.Add(studentId, examId);
        }

        public List<ExamResult> Get(ExamSlot exam)
        {
            return _examResults.Get(exam);
        }

        internal void Update(ExamResult examResult)
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
