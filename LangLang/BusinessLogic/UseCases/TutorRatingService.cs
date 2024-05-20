
using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

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

        public bool IsRated(int studentId, int tutorId)
        {
            return GetAll().Any(rating => rating.StudentId == studentId && rating.TutorId == tutorId);
        }

    }
}
