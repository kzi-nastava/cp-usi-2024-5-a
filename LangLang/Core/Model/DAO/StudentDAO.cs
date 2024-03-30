using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository;
using LangLang.Core.Observer;

namespace LangLang.Core.Model.DAO
{
    /**
     * This class encapsulates a list of Student objects and provides methods
     * for adding, updating, deleting, and retrieving Student objects.
     * Additionally, this class uses Repository<Student> for loading and saving objects.
    **/
    public class StudentDAO : Subject
    {
        private readonly Dictionary<int, Student> _students;
        private readonly Repository<Student> _repository;
    
    
        public StudentDAO()
        {
            _repository = new Repository<Student>("students.csv");
            _students = _repository.Load();
        }

        private int GenerateId()
        {
            if (_students.Count == 0) return 0;
            return _students.Count + 1;
        }

        public Student? GetStudentById(int id)
        {
            return _students[id];
        }

        public Dictionary<int, Student> GetAllStudents()
        {
            return _students;
        }

        public Student AddStudent(Student student)
        {
            student.Profile.Id = GenerateId();
            _students.Add(student.Profile.Id, student);
            _repository.Save(_students);
            NotifyObservers();
            return student;
        }

        public Student? UpdateStudent(Student student)
        {
            Student oldStudent = GetStudentById(student.Profile.Id);
            if (oldStudent == null) return null;

            oldStudent.Profile.Name = student.Profile.Name;
            oldStudent.Profile.LastName = student.Profile.LastName;
            oldStudent.Profile.Gender = student.Profile.Gender;
            oldStudent.Profile.DateOfBirth = student.Profile.DateOfBirth;
            oldStudent.Profile.PhoneNumber = student.Profile.PhoneNumber;
            oldStudent.Profile.Email = student.Profile.Email;
            oldStudent.Profile.Role = student.Profile.Role;
            oldStudent.Profile.Password = student.Profile.Password;
            oldStudent.CanModifyInfo = student.CanModifyInfo;
            oldStudent.ProfessionalQualification = student.ProfessionalQualification;

            _repository.Save(_students);
            NotifyObservers();
            return oldStudent;
        }
     
        public Student? RemoveStudent(int id)
        {
            Student student = GetStudentById(id);
            if (student == null) return null;

            _students.Remove(student.Profile.Id);
            _repository.Save(_students);
            NotifyObservers();
            return student;
        }
    }
}
