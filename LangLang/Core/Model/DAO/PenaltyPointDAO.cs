﻿using System;
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
        private void AddPenaltyPoint(Student student, Tutor tutor, Course course)
        {
            PenaltyPoint point = new PenaltyPoint(GenerateId(), student.Profile.Id, tutor.Profile.Id, course.Id, DateTime.Now);
            _points[point.Id] = point;
            _repository.Save(_points);
            NotifyObservers();
        }
        public void GivePenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
        {
            AddPenaltyPoint(student,tutor, course);
            if (ShouldDeactivate(student))
            {
                appController.StudentController.Delete(student, appController);
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
        public void RemovePenaltyPoint(Student student)
        {
            List<PenaltyPoint> points = GetPenaltyPoints(student);
            if (points.Count > 0)
            {
                PenaltyPoint toRemove = GetOldestPenaltyPoint(GetPenaltyPoints(student));
                _points.Remove(toRemove.Id);
                _repository.Save(_points);
                NotifyObservers();

            }
        }

        public PenaltyPoint GetOldestPenaltyPoint(List<PenaltyPoint> points)
        {           
            PenaltyPoint oldestPoint = null;
            DateTime oldestDate = DateTime.MaxValue;

            foreach (var point in points)
            {
                if (point.Date < oldestDate)
                {
                    oldestPoint = point;
                    oldestDate = point.Date;
                }
            }

            return oldestPoint;
        }

        public bool HasAlreadyGivenPenaltyPoint(Student student, Tutor tutor, Course course, AppController appController)
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
