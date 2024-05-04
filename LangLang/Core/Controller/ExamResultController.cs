using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
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

        public List<ExamResult> Get(int examId)
        {
            return _examResults.Get(examId);
        }

        internal void Update(ExamResult examResult)
        {
            _examResults.Update(examResult);
        }
    }
}
