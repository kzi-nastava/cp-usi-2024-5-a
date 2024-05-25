using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.BusinessLogic.UseCases
{
    public class GradeService
    {
        private IGradeRepository _grades;
        public GradeService()
        {
            _grades = Injector.CreateInstance<IGradeRepository>();
        }
        private int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }
        public Grade Get(int id)
        {
            return _grades.Get(id);
        }
        public void Add(Grade grade)
        {
            grade.Id = GenerateId();
            _grades.Add(grade);
        }
        public void Update(Grade grade)
        {
            _grades.Update(grade);
        }

        public void Delete(int id)
        {
            _grades.Delete(id);
        }
        public List<Grade> GetAll()
        {
            return _grades.GetAll();
        }
        public List<Grade> GetByStudent(Student student)
        {
            List<Grade> grades = new();

            foreach (Grade grade in GetAll())
            {
                if (grade.StudentId == student.Id)
                {
                    grades.Add(grade);
                }
            }

            return grades;
        }

        public List<Grade> GetByCourse(Course course)
        {
            List<Grade> grades = new List<Grade>();

            foreach (Grade grade in GetAll())
            {
                if (grade.CourseId == course.Id)
                {
                    grades.Add(grade);
                }
            }

            return grades;
        }
        public double GetAverageGrade(List<Student> students, Course course)
        {
            var grades = GetByCourse(course);
            var studentIds = students.Select(s => s.Id).ToHashSet(); // for a quick check of key existence

            var filteredGrades = grades.Where(g => studentIds.Contains(g.StudentId)).ToList();

            if (filteredGrades.Count == 0)
                return 0;

            double total = filteredGrades.Sum(g => (g.ActivityGrade + g.KnowledgeGrade) / 2);
            double average = total / filteredGrades.Count;

            return average;
        }
    }
}
