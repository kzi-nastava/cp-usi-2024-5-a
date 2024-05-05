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

        public Student GetById(int id)
        {
            return _students.GetStudentById(id);
        }

        public void Add(Student student)
        {
            _students.AddStudent(student);
        }

        public void Delete(Student student, AppController appController)
        {
            _students.RemoveStudent(student, appController);
        }

        public void Update(Student student)
        {
            _students.UpdateStudent(student);
        }

        public void Subscribe(IObserver observer)
        {
            _students.Subscribe(observer);
        }

        public bool CanModifyInfo(Student student, AppController appController)
        {
            return _students.CanModifyInfo(student, appController);
        }

        public bool CanRequestEnroll(int id, AppController appController)
        {
            return _students.CanRequestEnroll(id, appController);
        }
        //Checks if student can apply for courses (doesn't have exams with no generated results or not graded exams)
        public bool CanApplyForCourses(Student student, AppController appController)
        {
            return _students.CanApplyForCourses(student, appController);
        }
        //Checks if student can apply for exams (all students exams have final results)
        public bool CanApplyForExams(Student student, AppController appController)
        {
            return _students.CanApplyForExams(student, appController);
        }

    }
}
