using LangLang.BusinessLogic.UseCases;
using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Repository;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Controller
{
    public class PenaltyPointController
    {
        private readonly PenaltyPointDAO _points;

        public PenaltyPointController()
        {
            _points = new PenaltyPointDAO();
        }

        public PenaltyPoint? Get(int id)
        {
            return _points.Get(id);
        }
        public List<PenaltyPoint> GetAll()
        {
            return _points.GetAll().Values.ToList();
        }
        // Allows a tutor to assign a penalty point to a student for a specific course and deactivate the student's account if required
        public void GivePenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
        {
            _points.GivePenaltyPoint(student,tutor, course, appController);
        }

        //Returns list of penalty points of given student
        public List<PenaltyPoint> GetPenaltyPoints(Student student)
        {
            return _points.GetPenaltyPoints(student);
        }
        //Removes students oldest penalty point
        public void RemovePenaltyPoint(Student student)
        {
            _points.RemovePenaltyPoint(student);
        }

        public bool HasGivenPenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
        {
            return _points.HasGivenPenaltyPoint(student, tutor, course, appController);
        }

        public int CountByCourse(Course course)
        {
            return GetAll().Count(point => point.CourseId == course.Id);
        }

        public List<PenaltyPoint> GetByCourse(Course course)
        {
            return GetAll().Where(point => point.CourseId == course.Id).ToList();
        }

        public List<Student> GetStudentsByPenaltyCount(Course course, int penaltyCount)
        {
            List<Student> students = new();
            var studentService = new StudentService();
            foreach (var point in GetByCourse(course))
            {
                var student = studentService.Get(point.StudentId);
                if (HasNPenaltiesOnCourse(course, student.Id, penaltyCount))
                    students.Add(student);
            }
            return students;
        }

        private bool HasNPenaltiesOnCourse(Course course, int studentId, int n)
        {
            return n == GetAll().Count(point => point.CourseId == course.Id && point.StudentId == studentId);
        }
    }
}
