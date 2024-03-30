using LangLang.Core.Model;
using LangLang.Core.Repository;
using LangLang.Core.Observer;
using System.Collections.Generic;


namespace LangLang.Core.DAO
{
    public class TutorDAO : Subject
    {
        private readonly List<Tutor> _tutors;
        private readonly Repository<Tutor> _repository;

        public TutorDAO()
        {
            _repository = new Repository<Tutor>("tutors.csv");
            _tutors = _repository.Load();
        }
        private int GenerateId()
        {
            if (_tutors.Count == 0) return 0;
            return _tutors.Count + 1;
        }

        private Tutor Get(int id)
        {
            return _tutors.Find(t => t.Profile.Id == id);
        }

        public Tutor Add(Tutor tutor)
        {
            tutor.Profile.Id = GenerateId();
            _tutors.Add(tutor);
            _repository.Save(_tutors);
            NotifyObservers();
            return tutor;
        }

        public Tutor Update(Tutor tutor)
        {
            Tutor oldTutor = Get(tutor.Profile.Id);
            if (oldTutor == null) { return null; }

            oldTutor.Profile.Name = tutor.Profile.Name;
            oldTutor.Profile.LastName = tutor.Profile.LastName;
            oldTutor.Profile.Gender = tutor.Profile.Gender;
            oldTutor.Profile.DateOfBirth = tutor.Profile.DateOfBirth;
            oldTutor.Profile.PhoneNumber = tutor.Profile.PhoneNumber;
            oldTutor.Profile.Email = tutor.Profile.Email;
            oldTutor.Profile.Password = tutor.Profile.Password;
            oldTutor.Profile.Role = tutor.Profile.Role;

            _repository.Save(_tutors);
            NotifyObservers();
            return oldTutor;
        }

        // TODO: implement GetAllTutors and Remove method

    }
}

