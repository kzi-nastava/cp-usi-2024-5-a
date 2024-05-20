using LangLang.BusinessLogic.UseCases;
using LangLang.Core;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangLang.Repositories
{
    public class StudentRepository : Subject, IStudentRepository
    {
        private readonly Dictionary<int, Student> _students;
        private const string _filePath = Constants.FILENAME_PREFIX + "students.csv";

        public StudentRepository() {
            _students = Load();
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
            NotifyObservers();
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
            NotifyObservers();
        }

        public void Deactivate(int id)
        {
            Student student = Get(id);
            if (student == null) return;

            var enrollmentService = new EnrollmentRequestService();
            var examAppService = new ExamApplicationController();
            var examService = new ExamSlotController();

            foreach (EnrollmentRequest er in enrollmentService.GetByStudent(student)) // delete all course enrollment requests
                enrollmentService.Delete(er.Id);

            foreach (ExamApplication ar in examAppService.GetApplications(student)) // delete all exam application requests
                examAppService.Delete(ar.Id, examService);

            _students[id].Profile.IsActive = false;
            Save();
            NotifyObservers();
        }

        public void Save()
        {
            {
                using var writer = new StreamWriter(_filePath);
                
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

        public Dictionary<int, Student> Load()
        {
            var students = new Dictionary<int, Student>();

            if (!File.Exists(_filePath)) return students;
            
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var tokens = line.Split(Constants.DELIMITER);

                var profile = new Profile(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], 
                                          tokens[5], tokens[6], tokens[7], tokens[8], tokens[9]);
                var student = new Student(profile, tokens[10]);
                students.Add(student.Id, student);
            }

            return students;
        }

    }
}
