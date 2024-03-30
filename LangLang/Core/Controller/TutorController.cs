using LangLang.Core.DAO;
using LangLang.Core.Model;
using LangLang.Core.Observer;
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

        public Dictionary<int, Tutor> GetAllTutors()
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

        public void Subscribe(IObserver observer)
        {
            _tutors.Subscribe(observer);
        }
    }
}
