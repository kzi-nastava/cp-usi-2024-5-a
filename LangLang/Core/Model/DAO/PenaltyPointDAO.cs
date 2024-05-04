using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Core.Controller;
using LangLang.Core.Observer;
using LangLang.Core.Repository;

namespace LangLang.Core.Model.DAO
{
    public class PenaltyPointDAO: Subject
    {

        private readonly Dictionary<int, PenaltyPoint> _points;
        private readonly Repository<PenaltyPoint> _repository;

        public PenaltyPointDAO()
        {
            _repository = new Repository<PenaltyPoint>("penaltyPoints.csv");
            _points = _repository.Load();
        }
        private int GenerateId()
        {
            if (_points.Count == 0) return 0;
            return _points.Keys.Max() + 1;
        }

        public PenaltyPoint? GetById(int id)
        {
            return _points[id];
        }
        public Dictionary<int, PenaltyPoint> GetAllPoints()
        {
            return _points;
        }

        // Allows a tutor to assign a penalty point to a student for a specific course and deactivate the student's account if required
        public void GivePenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
        {
            PenaltyPoint point = new PenaltyPoint(GenerateId(),student.Profile.Id, tutor.Profile.Id, course.Id,DateTime.Now);
            _points[point.Id] = point;
            _repository.Save(_points);
            NotifyObservers();

            appController.StudentController.GivePenaltyPoint(student, appController);
            
        }

        public List<PenaltyPoint> GetPenaltyPoints(Student student)
        {
            List<PenaltyPoint> points = new List<PenaltyPoint>();

            foreach (var point in _points.Values)
            {
                if (point.StudentId == student.Profile.Id)
                {
                    points.Add(point);
                }
            }

            return points;
        }
        public void RemovePenaltyPoint(Student student, AppController appController)
        {
            List<PenaltyPoint> points = GetPenaltyPoints(student);
            if (points.Count > 0)
            {
                _points.Remove(points[0].Id);
                _repository.Save(_points);
                NotifyObservers();

                appController.StudentController.RemovePenaltyPoint(student);

            }
        }

    }
}
