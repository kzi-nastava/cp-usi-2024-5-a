using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using System.Collections.Generic;
using LangLang.Domain.Models;
using System.Drawing;
using System.Linq;

namespace LangLang.Core.Controller
{
    public class GradeController
    {
        private readonly GradeDAO _grades;

        public GradeController()
        {
            _grades = new GradeDAO();
        }

        public List<Grade> GetAll()
        {
            return _grades.GetAll();
        }

        public List<Grade> GetByStudent(Student student)
        {
            return _grades.GetByStudent(student);
        }

        public List<Grade> GetByCourse(Course course)
        {
            return _grades.GetByCourse(course);
        }

        public void Add(Grade grade)
        {
            _grades.Add(grade);
        }

        public void Update(Grade grade)
        {
            _grades.Update(grade);
        }

        public void Delete(int gradeId)
        {
            _grades.Remove(gradeId);    
        }

        public double GetAverageGrade(List<Student> students, Course course)
        {
            var grades = GetByCourse(course); 
            var studentIds = students.Select(s => s.Id).ToHashSet(); // for a quick check of key existence

            var filteredGrades = grades.Where(g => studentIds.Contains(g.StudentId)).ToList();

            if (filteredGrades.Count == 0) 
                return 0;

            double total = filteredGrades.Sum(g => (g.ActivityGrade + g.KnowledgeGrade)/2);
            double average = total / filteredGrades.Count;

            return average;
        }
    }
}
