using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangLang.Repositories.FileRepositories
{
    public class StudentRepository : IStudentRepository
    {
        private Dictionary<int, Student> _students;
        private const string _filePath = Constants.FILENAME_PREFIX + "students.csv";

        public StudentRepository()
        {
            Load();
        }
        public Student Get(int id)
        {
            return _students[id];
        }

        public List<Student> GetAll()
        {
            return _students.Values.ToList();
        }

        public void Add(Student student)
        {
            _students.Add(student.Profile.Id, student);
            Save();
        }

        public void Update(Student student)
        {
            Student? oldStudent = Get(student.Profile.Id);
            if (oldStudent == null) return;

            oldStudent.Profile.Id = student.Profile.Id;
            oldStudent.Profile.Name = student.Profile.Name;
            oldStudent.Profile.LastName = student.Profile.LastName;
            oldStudent.Profile.Gender = student.Profile.Gender;
            oldStudent.Profile.BirthDate = student.Profile.BirthDate;
            oldStudent.Profile.PhoneNumber = student.Profile.PhoneNumber;
            oldStudent.Profile.Email = student.Profile.Email;
            oldStudent.Profile.Role = student.Profile.Role;
            oldStudent.Profile.Password = student.Profile.Password;
            oldStudent.Profession = student.Profession;

            Save();
        }

        public void Deactivate(int id)
        {
            Student student = Get(id);
            if (student == null) return;

            _students[id].Profile.IsActive = true;
            Save();
        }

        public void Save()
        {
            using (var writer = new StreamWriter(_filePath))
            {
                foreach (var student in GetAll())
                {
                    var profile = student.Profile;
                    var line = string.Join(Constants.DELIMITER.ToString(),
                                        profile.ToString(),
                                        student.Profession);
                    writer.WriteLine(line);
                }
            }
        }

        public void Load()
        {
            _students = new Dictionary<int, Student>();

            if (!File.Exists(_filePath)) return;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var tokens = line.Split(Constants.DELIMITER);

                var profile = new Profile(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4],
                                          tokens[5], tokens[6], tokens[7], tokens[8], tokens[9]);
                var student = new Student(profile, tokens[10]);
                _students.Add(student.Id, student);
            }
        }

    }
}
