using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using System.Collections.Generic;
using LangLang.Domain.Models;

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
    }
}
