using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

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
        public List<ExamResult> GetByStudent(Student student)
        {
            return _examResults.GetByStudent(student);
        }
        public List<ExamResult> GetByExams(List<ExamSlot> exams)
        {
            List<ExamResult> results = new();
            foreach(ExamSlot exam in exams)
            {
                results.AddRange(GetByExam(exam));
            }
            return results;
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
        public bool IsResultForCourse(ExamResult result,Course course)
        {
            var examService = new ExamSlotService();
            var languageService = new LanguageLevelService();

            ExamSlot exam = examService.Get(result.ExamSlotId);
            var examLanguage = languageService.Get(exam.LanguageId);

            if (examLanguage.Language == course.Language && examLanguage.Level == course.Level)
                return true;
            return false;
        }
    }
}
