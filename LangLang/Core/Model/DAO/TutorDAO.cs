﻿using LangLang.Core.Model;
using LangLang.Core.Repository;
using LangLang.Core.Observer;
using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Controller;
using System;

namespace LangLang.Core.DAO
{
    public class TutorDAO : Subject
    {
        private readonly Dictionary<int, Tutor> _tutors;
        private readonly Repository<Tutor> _repository;

        public TutorDAO()
        {
            _repository = new Repository<Tutor>("tutors.csv");
            _tutors = _repository.Load();
        }
        private int GenerateId()
        {
            if (_tutors.Count == 0) return 0;
            return _tutors.Keys.Max() + 1;
        }

        private Tutor? Get(int id)
        {
            return _tutors[id];
        }

        public Tutor Add(Tutor tutor)
        {
            tutor.Profile.Id = GenerateId();
            _tutors.Add(tutor.Profile.Id, tutor);
            _repository.Save(_tutors);
            NotifyObservers();
            return tutor;
        }

        public Dictionary<int, Tutor> Search(TutorController tutorController, DateTime date, string language, LanguageLevel? level)
        {
            Dictionary<int, Tutor> allTutors = tutorController.GetAllTutors();

            Dictionary<int, Tutor> filteredTutors = allTutors.Where(tutor =>
            (date == default || tutor.Value.EmploymentDate.Date == date.Date) &&
             (language == "" || tutor.Value.Skill.Language.Any(skill => skill.Contains(language))) &&
             (level == null || tutor.Value.Skill.Level.Any(skilll => skilll == level))).ToDictionary(x => x.Key, x => x.Value);


            return filteredTutors;
        }
        public Tutor? Update(Tutor tutor)
        {
            Tutor oldTutor = Get(tutor.Profile.Id);
            if (oldTutor == null) { return null; }

            oldTutor.Profile.Name = tutor.Profile.Name;
            oldTutor.Profile.LastName = tutor.Profile.LastName;
            oldTutor.Profile.Gender = tutor.Profile.Gender;
            oldTutor.Profile.BirthDate = tutor.Profile.BirthDate;
            oldTutor.Profile.PhoneNumber = tutor.Profile.PhoneNumber;
            oldTutor.Profile.Email = tutor.Profile.Email;
            oldTutor.Profile.Password = tutor.Profile.Password;
            oldTutor.Profile.Role = tutor.Profile.Role;
            oldTutor.EmploymentDate = tutor.EmploymentDate;

            _repository.Save(_tutors);
            NotifyObservers();
            return oldTutor;
        }

        public Dictionary<int, Tutor> GetAllTutors()
        {
            return _tutors;
        }

        public Tutor? Remove(int id)
        {
            Tutor tutor = Get(id);
            if (tutor == null) return null;

            _tutors.Remove(tutor.Profile.Id);
            _repository.Save(_tutors);
            NotifyObservers();
            return tutor;
        }
    }
}

