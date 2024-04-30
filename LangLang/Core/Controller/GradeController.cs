using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Controller
{
    public class GradeController
    {
        private readonly GradeDAO _grades;

        public GradeController()
        {
            _grades = new GradeDAO();
        }

        public Dictionary<int, Grade> GetAllGrades()
        {
            return _grades.GetAllGrades();
        }

        public Dictionary<int, Grade> GetGradesWithStudent(int studentId)
        {
            return _grades.GetGradesWithStudent(studentId);
        }

        public Dictionary<int, Grade> GetGradesWithCourse(int courseId)
        {
            return _grades.GetGradesWithCourse(courseId);
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
    }
}
