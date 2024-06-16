using LangLang.Composition;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using LangLang.Domain.Enums;

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

        public int Add(Tutor tutor)
        {
            tutor.Profile.Id = GenerateId();
            return _tutors.Add(tutor);
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

        public List<Tutor> Search(DateTime date, string? language, Level? level)
        {
            var tutorSkillService = new TutorSkillService();
            List<Tutor> allTutors = GetAll();

            return allTutors.Where(tutor =>
            {
                bool dateMatches = date == default || tutor.EmploymentDate.Date == date.Date;

                bool isActive = tutor.Profile.IsActive == true;

                bool languageAndLevelMatch = true;
                if (!string.IsNullOrEmpty(language) || level.HasValue)
                {
                    List<LanguageLevel> tutorSkills = tutorSkillService.GetByTutor(tutor);
                    languageAndLevelMatch = tutorSkills.Any(skill =>
                        (string.IsNullOrEmpty(language) || skill.Language.Contains(language, StringComparison.OrdinalIgnoreCase)) &&
                        (!level.HasValue || skill.Level.ToString().Contains(level.Value.ToString(), StringComparison.OrdinalIgnoreCase)));
                }

                return dateMatches && isActive && languageAndLevelMatch;
            }).ToList();
        }

        public Tutor GetByEmail(string email)
        {
            return _tutors.GetByEmail(email);
        }

    }
}
