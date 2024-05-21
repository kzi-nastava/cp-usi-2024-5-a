using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.BusinessLogic.UseCases;
using LangLang.Composition;
using LangLang.Core.Controller;
using LangLang.Core.Observer;
using LangLang.Core.Repository;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Core.Model.DAO
{
    public class PenaltyPointService: Subject
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
        public void GivePenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
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

        public bool HasGivenPenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
        {
            List<PenaltyPoint> studentPenaltyPoints = GetPenaltyPoints(student);
            foreach(PenaltyPoint point in studentPenaltyPoints)
            {
                if (point.CourseId == course.Id && point.Date.Date == DateTime.Now.Date) return true;
            }
            return false;
        }

    }
}
