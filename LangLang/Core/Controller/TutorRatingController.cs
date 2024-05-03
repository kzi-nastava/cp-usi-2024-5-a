using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Observer;
using System.Collections.Generic;

namespace LangLang.Core.Controller
{
    public class TutorRatingController 
    {
        private readonly TutorRatingDAO _tutorRatings;

        public TutorRatingController()
        {
            _tutorRatings = new();
        }

        public List<TutorRating> GetAll()
        {
            return _tutorRatings.GetAll();
        }

        public TutorRating GetById(int id)
        {
            return _tutorRatings.GetById(id);
        }

        public TutorRating Add(TutorRating rating)
        {
            return _tutorRatings.Add(rating);
        }

        public void Subscribe(IObserver observer)
        {
            _tutorRatings.Subscribe(observer);
        }

        public bool IsRated(int studentId, int tutorId) 
        {
            return _tutorRatings.IsRated(studentId, tutorId);
        }
    }
}
