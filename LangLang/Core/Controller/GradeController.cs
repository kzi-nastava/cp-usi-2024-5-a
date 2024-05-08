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

        public Dictionary<int, Grade> GetAll()
        {
            return _grades.GetAll();
        }

        public Dictionary<int, Grade> GetByStudent(Student student)
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
