using LangLang.Core.Observer;
using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Model.DAO
{
    public class TutorRatingDAO : Subject
    {
        private readonly Dictionary<int, TutorRating> _tutorRatings;
        private readonly Repository<TutorRating> _repository;

        public TutorRatingDAO()
        {
            _repository = new Repository<TutorRating>("tutorRatings.csv");
            _tutorRatings = _repository.Load();
        }

        private int GenerateId()
        {
            if (_tutorRatings.Count == 0) return 0;
            return _tutorRatings.Keys.Max() + 1;
        }

        public TutorRating GetById(int id)
        {
            return _tutorRatings[id];
        }

        public List<TutorRating> GetAll()
        {
            return _tutorRatings.Values.ToList();
        }

        public TutorRating Add(TutorRating rating)
        {
            if (rating.Rating < 1 || rating.Rating > 10)
            {
                throw new ArgumentException("Rating must be between 1 and 10");
            }

            rating.Id = GenerateId();
            _tutorRatings.Add(rating.Id, rating);
            _repository.Save(_tutorRatings);
            NotifyObservers();
            return rating;
        }

        public bool IsRated(int studentId, int tutorId)
        {
            return GetAll().Any(rating => rating.StudentId == studentId && rating.TutorId == tutorId);
        }

    }
}
