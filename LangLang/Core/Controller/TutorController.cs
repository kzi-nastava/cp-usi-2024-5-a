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

        public List<Tutor> GetAll()
        {
            return _tutors.GetAll();
        }

        public void Add(Tutor tutor)
        {
            _tutors.Add(tutor);
        }

        public void Deactivate(int tutorId)
        {
            _tutors.Deactivate(tutorId);
        }

        public List<Tutor> Search(string language, DateTime date, LanguageLevel? level)
        {
            return _tutors.Search(date, language, level);
        }
        public void Update(Tutor tutor)
        {
            _tutors.Update(tutor);
        }
        public void Subscribe(IObserver observer)
        {
            _tutors.Subscribe(observer);
        }

        public Tutor Get(int id)
        {
            return _tutors.Get(id);
        }
    }
}
