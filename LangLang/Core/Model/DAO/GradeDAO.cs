using LangLang.Core.Observer;
using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Model.DAO
{
    public class GradeDAO: Subject
    {
        private readonly Dictionary<int, Grade> _grades;
        private readonly Repository<Grade> _repository;

        public GradeDAO()
        {
            _repository = new Repository<Grade>("grades.csv");
            _grades = _repository.Load();
        }

        private int GenerateId()
        {
            if (_grades.Count == 0) return 0;
            return _grades.Keys.Max() + 1;
        }

        public Grade Add(Grade grade)
        {
            grade.Id = GenerateId();
            _grades[grade.Id] = grade;
            _repository.Save(_grades);
            NotifyObservers();
            return grade;
        }

        public Grade Update(Grade grade)
        {
            Grade oldGrade = _grades[grade.Id];
            if (oldGrade == null) return null;

            oldGrade.StudentId = grade.StudentId;
            oldGrade.CourseId = grade.CourseId;
            oldGrade.GradeValue = grade.GradeValue;

            _repository.Save(_grades);
            NotifyObservers();
            return oldGrade;
        }

        public Grade Remove(int id)
        {
            Grade grade = _grades[id];
            if (grade == null) return null;

            _grades.Remove(id);
            _repository.Save(_grades);
            NotifyObservers();
            return grade;
        }

        public Dictionary<int, Grade> GetAll()
        {
            return _grades;
        }

        private Grade Get(int id)
        {
            return _grades[id];
        }

        public Dictionary<int, Grade> GetByStudent(Student student)
        {
            Dictionary<int, Grade> grades = new Dictionary<int, Grade>();

            foreach (Grade grade in _grades.Values)
            {
                if (grade.StudentId == student.Id)
                {
                    grades[grade.Id] = grade;
                }
            }

            return grades;
        }
        public List<Grade> GetByCourse(Course course)
        {
            List<Grade> grades = new List<Grade>();

            foreach (Grade grade in _grades.Values)
            {
                if (grade.CourseId == course.Id)
                {
                    grades.Add(grade);
                }
            }

            return grades;
        }
    }
}
