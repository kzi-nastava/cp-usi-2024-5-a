using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

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
            return GetAll().Where(grade => grade.CourseId == course.Id).ToList();
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
        public double GetAverageKnowledgeGrade(Course course)
        {
            List<int> knowledgeGrades = new();
            foreach (Grade grade in GetByCourse(course))
            {
                knowledgeGrades.Add(grade.KnowledgeGrade);
            }
            if (knowledgeGrades.Count == 0) return 0;
            return knowledgeGrades.Average();
        }

        public double GetAverageActivityGrade(Course course)
        {
            List<int> activityGrades = new();
            foreach (Grade grade in GetByCourse(course))
            {
                activityGrades.Add(grade.ActivityGrade);
            }
            if (activityGrades.Count == 0) return 0;
            return activityGrades.Average();
        }


        public bool IsGraded(Course course)
        {
            return GetAll().Any(grade => grade.CourseId == course.Id);
        }

        public int CountGradedStudents(Course course)
        {
            return GetAll().Count(grade => grade.CourseId == course.Id);
        }
    }
}
