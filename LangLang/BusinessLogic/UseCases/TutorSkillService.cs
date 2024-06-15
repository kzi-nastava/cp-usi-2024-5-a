
using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.BusinessLogic.UseCases
{
    public class TutorSkillService
    {
        private ITutorSkillRepository tutorSkillRepository;

        public TutorSkillService()
        {
            tutorSkillRepository = Injector.CreateInstance<ITutorSkillRepository>();
        }

        public List<TutorSkill> GetAll()
        {
            return tutorSkillRepository.GetAll();
        }

        public TutorSkill Get(int id)
        {
            return tutorSkillRepository.Get(id);
        }

        public int Add(TutorSkill tutorSkill)
        {
            return tutorSkillRepository.Add(tutorSkill);
        }

        public void Delete(TutorSkill tutorSkill)
        {
            tutorSkillRepository.Delete(tutorSkill);
        }

        public List<Tutor> GetBySkill(LanguageLevel languageLevel)
        {
            return tutorSkillRepository.GetBySkill(languageLevel);
        }

        public List<LanguageLevel> GetByTutor(Tutor tutor)
        {
            return tutorSkillRepository.GetByTutor(tutor);
        }

        public bool AlreadyAdded(int tutorId, int skillId)
        {
            return tutorSkillRepository.AlreadyAdded(tutorId, skillId);
        }
    }
}
