using LangLang.Core.DAO;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using System;
using System.Collections.Generic;

namespace LangLang.Core.Controller
{
    public class TutorController
    {
        private readonly TutorDAO _tutors;

        public TutorController()
        {
            _tutors = new TutorDAO();
        }

        public List<Tutor> GetAllTutors()
        {
            return _tutors.GetAllTutors();
        }

        public void Add(Tutor tutor)
        {
            _tutors.Add(tutor);
        }

        public void Delete(int tutorId)
        {
            _tutors.Remove(tutorId);
        }

        public List<Tutor> Search(string language, DateTime date, LanguageLevel? level)
        {
            return _tutors.Search(this, date, language, level);
        }
        public void Update(Tutor tutor)
        {
            _tutors.Update(tutor);
        }
        public void Subscribe(IObserver observer)
        {
            _tutors.Subscribe(observer);
        }
    }
}
