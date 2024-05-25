using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Composition;
using LangLang.Configuration;
using LangLang.Core.Observer;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.BusinessLogic.UseCases
{
    public class PenaltyPointService
    {

        
        private IPenaltyPointRepository _points;

        public PenaltyPointService()
        {
            _points = Injector.CreateInstance<IPenaltyPointRepository>();
        }

        private int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public PenaltyPoint? Get(int id)
        {
            return _points.Get(id);
        }
        public List<PenaltyPoint> GetAll()
        {
            return _points.GetAll();
        }
        private void Add(Student student, Tutor tutor, Course course)
        {
            PenaltyPoint point = new PenaltyPoint(GenerateId(), student.Profile.Id, tutor.Profile.Id, course.Id, DateTime.Now);
            _points.Add(point);
        }
        public void GivePenaltyPoint(Student student, Tutor tutor, Course course)
        {
            Add(student,tutor, course);
            if (ShouldDeactivate(student))
            {
                var studentService = new StudentService();
                studentService.Deactivate(student.Id);
            }
        }
        private bool ShouldDeactivate(Student student)
        {
            List<PenaltyPoint> studentPoints = GetPenaltyPoints(student);
            if(studentPoints.Count == Constants.MAX_PENALTY_POINTS)
            {
                return true;
            }
            return false;
        }
        public List<PenaltyPoint> GetPenaltyPoints(Student student)
        {
            return _points.GetPenaltyPoints(student);
        }
        public void Delete(PenaltyPoint point)
        {
            _points.Delete(point);
        }
        public void RemovePenaltyPoint(Student student)
        {
            List<PenaltyPoint> points = GetPenaltyPoints(student);
            if (points.Count > 0)
            {
                PenaltyPoint toRemove = GetOldestPenaltyPoint(GetPenaltyPoints(student));
                Delete(toRemove);

            }
        }

        public PenaltyPoint GetOldestPenaltyPoint(List<PenaltyPoint> points)
        {
            return _points.GetOldestPenaltyPoint(points);
        }

        public bool HasGivenPenaltyPoint(Student student, Tutor tutor, Course course)
        {
            List<PenaltyPoint> studentPenaltyPoints = GetPenaltyPoints(student);
            foreach(PenaltyPoint point in studentPenaltyPoints)
            {
                if (point.CourseId == course.Id && point.Date.Date == DateTime.Now.Date) return true;
            }
            return false;
        }
        private bool HasNPenaltiesOnCourse(Course course, int studentId, int n)
        {
            return n == GetAll().Count(point => point.CourseId == course.Id && point.StudentId == studentId);
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
        public int CountByCourse(Course course)
        {
            return GetAll().Count(point => point.CourseId == course.Id);
        }

        public List<PenaltyPoint> GetByCourse(Course course)
        {
            return GetAll().Where(point => point.CourseId == course.Id).ToList();
        }
    }
}
