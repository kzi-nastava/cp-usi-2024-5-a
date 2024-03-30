using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using System.Collections.Generic;
using LangLang.Core.Observer;

namespace LangLang.Core.Controller
{
    public class StudentController
    {
        private readonly StudentDAO _students;

        public StudentController()
        {
            _students = new StudentDAO();
        }

        public List<Student> GetAllStudents()
        {
            return _students.GetAllStudents();
        }

        public void Add(Student student)
        {
            _students.AddStudent(student);
        }

        public void Delete(int studentId)
        {
            _students.RemoveStudent(studentId);
        }

        public void Subscribe(IObserver observer)
        {
            _students.Subscribe(observer);
        }
    }
}
