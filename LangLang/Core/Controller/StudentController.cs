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

        public List<Student> GetAll()
        {
            return _students.GetAll();
        }

        public Student Get(int id)
        {
            return _students.Get(id);
        }

        public void Add(Student student)
        {
            _students.Add(student);
        }

        public void Delete(int id, AppController appController)
        {
            _students.Remove(id, appController);
        }

        public void Update(Student student)
        {
            _students.Update(student);
        }

        public void Subscribe(IObserver observer)
        {
            _students.Subscribe(observer);
        }

        public bool CanModifyData(Student student, AppController appController)
        {
            return _students.CanModifyData(student, appController);
        }

        public bool CanRequestEnrollment(Student student, AppController appController)
        {
            return _students.CanRequestEnrollment(student.Id, appController);
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
