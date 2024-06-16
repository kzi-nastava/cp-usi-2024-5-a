using LangLang.Domain.RepositoryInterfaces;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using LangLang.Domain.Enums;
using LangLang.Configuration;

namespace LangLang.BusinessLogic.UseCases
{
    public class TutorService
    {
        private ITutorRepository _tutors;

        public TutorService()
        {
            _tutors = Injector.CreateInstance<ITutorRepository>();
        }

        public int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public List<Tutor> GetAll()
        {
            return _tutors.GetAll();
        }

        public Tutor Get(int id)
        {
            return _tutors.Get(id);
        }

        public void Add(Tutor tutor)
        {
            tutor.Profile.Id = GenerateId();
            _tutors.Add(tutor);
        }

        public void Update(Tutor tutor)
        {
            _tutors.Update(tutor);
        }

        public void Deactivate(int id)
        {
            _tutors.Deactivate(id);
        }

        public List<Tutor> GetActive()
        {
            return _tutors.GetActive();
        }

        public List<Tutor> GetBySkill(string language, LanguageLevel level)
        {
            List<Tutor> tutors = new();
            foreach (Tutor tutor in GetActive())
            {
                for (int i = 0; i < tutor.Skill.Language.Count; i++)
                {
                    if (tutor.Skill.Language[i] == language && tutor.Skill.Level[i] == level)
                    {
                        tutors.Add(tutor);
                        break;
                    }
                }
            }
            return tutors;
        }

        public List<Tutor> Search(DateTime date, string language, LanguageLevel? level)
        {
            List<Tutor> allTutors = GetAll();

            return allTutors.Where(tutor =>
            (date == default || tutor.EmploymentDate.Date == date.Date) &&
             (language == "" || tutor.Skill.Language.Any(skill => skill.Contains(language))) &&
             (level == null || tutor.Skill.Level.Any(skill => skill == level)) &&
             (tutor.Profile.IsActive == true)).ToList();
        }

    }
}
