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
        private readonly List<Student> _students;
        private readonly Repository<Student> _repository;
    
    
        public StudentDAO()
        {
            _repository = new Repository<Student>("student.csv");
            _students = _repository.Load();
        }

        private int GenerateId()
        {
            if (_students.Count == 0) return 0;
            return _students.Last().Profile.Id + 1;
        }

        public Student AddStudent(Student student)
        {
            student.Profile.Id = GenerateId();
            _students.Add(student);
            _repository.Save(_students);
            NotifyObservers();
            return student;
        }

        public Student? UpdateStudent(Student student)
        {
            Student oldStudent = GetStudentById(student.Profile.Id);
            if (oldStudent == null) return null;

            oldStudent.Profile = student.Profile;
            oldStudent.CanModifyInfo = student.CanModifyInfo;
            oldStudent.ProfessionalQualification = student.ProfessionalQualification;

            _repository.Save(_students);
            NotifyObservers();
            return oldStudent;
        }
     
        public Student GetStudentById(int id)
        {
            return _students.Find(s => s.Profile.Id == id);
        }

        // TODO: implement RemoveStudent(int id), GetAllStudent()
    }

}
