using LangLang.Core;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.IO;

namespace LangLang.Repositories
{
    public class StudentRepository : Subject, IStudentRepository
    {
        private readonly Dictionary<int, Student> _students;
        private string _filePath = Constants.FILENAME_PREFIX + "students.csv";

        public StudentRepository() {
            _students = Load();
        }
        public Student Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Student> GetAll()
        {
            throw new System.NotImplementedException();
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

        public void Deactivate(int id, AppController appController)
        {
            Student student = Get(id);
            if (student == null) return;

            var enrollmentController = appController.EnrollmentRequestController;
            var examAppController = appController.ExamApplicationController;
            var examController = appController.ExamSlotController;

            foreach (EnrollmentRequest er in enrollmentController.GetRequests(student)) // delete all course enrollment requests
                enrollmentController.Delete(er.Id);

            foreach (ExamApplication ar in examAppController.GetApplications(student)) // delete all exam application requests
                examAppController.Delete(ar.Id, examController);

            _students[id].Profile.IsActive = true;
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
