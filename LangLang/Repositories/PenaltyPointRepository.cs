using LangLang.BusinessLogic.UseCases;
using LangLang.Core;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using LangLang.Core.Observer;
using LangLang.Core.Repository;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangLang.Repositories
{
    public class PenaltyPointRepository : Subject, IPenaltyPointRepository
    {
        private readonly Dictionary<int, PenaltyPoint> _points;
        private const string _filePath = Constants.FILENAME_PREFIX + "penaltyPoints.csv";

        public PenaltyPointRepository()
        {
            _points = Load();
        }

        public PenaltyPoint? Get(int id)
        {
            return _points[id];
        }

        public List<PenaltyPoint> GetAll()
        {
            return _points.Values.ToList();
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
        public void Add(PenaltyPoint point)
        {
            _points[point.Id] = point;
            Save();
            NotifyObservers();
        }
        public void Delete(PenaltyPoint point)
        {
            _points.Remove(point.Id);
            Save();
            NotifyObservers();
        }
        public void Save()
        {
            var lines = GetAll().Select(point =>
            {
                return string.Join(Constants.DELIMITER,
                    point.Id.ToString(),
                    point.StudentId.ToString(),
                    point.TutorId.ToString(),
                    point.CourseId.ToString(),
                    point.Date.ToString());
            });

            File.WriteAllLines(_filePath, lines);
        }

        public Dictionary<int, PenaltyPoint> Load()
        {
            var points = new Dictionary<int, PenaltyPoint>();

            if (!File.Exists(_filePath)) return points;

            var lines = File.ReadAllLines(_filePath);

            foreach (var line in lines)
            {
                string[] values = line.Split(Constants.DELIMITER);

                int id = int.Parse(values[0]);
                int studentId = int.Parse(values[1]);
                int tutorId = int.Parse(values[2]);
                int courseId = int.Parse(values[3]);
                DateTime date = DateTime.Parse(values[4]);
                PenaltyPoint point = new PenaltyPoint( id, studentId, tutorId, courseId, date);
                points.Add(point.Id, point);

            }

            return points;

        }
    }
}