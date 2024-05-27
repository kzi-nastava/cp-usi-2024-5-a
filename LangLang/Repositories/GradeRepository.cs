using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangLang.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly Dictionary<int, Grade> _grades;
        private string _filePath = Constants.FILENAME_PREFIX + "grades.csv";

        public GradeRepository()
        {
            _grades = Load();
        }
        public void Add(Grade grade)
        {
            _grades.Add(grade.Id, grade);
            Save();
        }

        public void Delete(int id)
        {
            Grade grade = _grades[id];
            if (grade == null) return;
            _grades.Remove(id);
            Save();
        }

        public Grade Get(int id)
        {
            return _grades[id];
        }

        public List<Grade> GetAll()
        {
            return _grades.Values.ToList(); 
        }

        public Dictionary<int, Grade> Load()
        {
            var grades = new Dictionary<int, Grade>();

            if (!File.Exists(_filePath)) return grades;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var tokens = line.Split(Constants.DELIMITER);
                int id = int.Parse(tokens[0]);
                int courseId = int.Parse(tokens[1]);
                int studentId = int.Parse(tokens[2]);
                int activityGrade = int.Parse(tokens[3]);
                int knowledgeGrade = int.Parse(tokens[4]);

                var grade = new Grade(id, courseId, studentId, activityGrade, knowledgeGrade);
                grades.Add(id, grade);
            }
            return grades;
        }

        public void Save()
        {
            using var writer = new StreamWriter(_filePath);

            foreach (var grade in GetAll())
            {
                var line = grade.ToString();
                writer.WriteLine(line);
            }
        }

        public void Update(Grade grade)
        {
            Grade oldGrade = _grades[grade.Id];
            if (oldGrade == null) return;

            oldGrade.StudentId = grade.StudentId;
            oldGrade.CourseId = grade.CourseId;
            oldGrade.ActivityGrade = grade.ActivityGrade;
            oldGrade.KnowledgeGrade = grade.KnowledgeGrade;

            Save();
        }
    }
}
