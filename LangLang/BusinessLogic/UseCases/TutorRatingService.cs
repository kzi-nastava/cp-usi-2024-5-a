
using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace LangLang.BusinessLogic.UseCases
{
    public class TutorRatingService
    {
        private ITutorRatingRepository _tutorRatings;

        public TutorRatingService()
        {
            _tutorRatings = Injector.CreateInstance<ITutorRatingRepository>();
        }
        public int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public TutorRating Get(int id)
        {
            return _tutorRatings.Get(id);
        }

        public List<TutorRating> GetAll()
        {
            return _tutorRatings.GetAll();
        }

        public void Add(TutorRating rating)
        {
            rating.Id = GenerateId();
            _tutorRatings.Add(rating);
        }

        public int GetId(int studentId, int courseId)
        {
            foreach (TutorRating rating in GetAll())
            {
                if (rating.CourseId == courseId && rating.StudentId == studentId) return rating.Id;
            }
            return -1;
        }
        public bool IsRated(int studentId, int courseId)
        {
            return GetId(studentId, courseId) != -1;
        }

        public double GetAverageTutorRating(Course course)
        {
            CourseService courseService = new();
            List<int> ratings = new();
            foreach (Student student in courseService.GetStudentsAttended(course))
            {
                int ratingId = GetId(student.Id, course.Id);
                if (ratingId != -1)
                {
                    ratings.Add(Get(ratingId).Rating);
                }
            }
            if (ratings.Count == 0) return 0;
            return ratings.Average();
        }
    }
}
