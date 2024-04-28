using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository;
using LangLang.Core.Observer;
using LangLang.Core.Controller;

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
            return _students.Keys.Max() + 1;
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

            _repository.Save(_students);
            NotifyObservers();
            return oldStudent;
        }
     
        public Student? RemoveStudent(int id, EnrollmentRequestController enrollmentRequestController)
        {
            Student student = GetStudentById(id);
            if (student == null) return null;

            foreach (EnrollmentRequest er in enrollmentRequestController.GetStudentRequests(id).Values)
            {
                enrollmentRequestController.Delete(er.Id);
            }

            _students.Remove(student.Profile.Id);
            _repository.Save(_students);
            NotifyObservers();
            return student;
        }
    }
}
