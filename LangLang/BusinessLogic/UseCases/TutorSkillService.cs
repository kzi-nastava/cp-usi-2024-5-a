
using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;

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

        public void Add(TutorSkill tutorSkill)
        {
            tutorSkillRepository.Add(tutorSkill);
        }

        public void Delete(TutorSkill tutorSkill)
        {
            tutorSkillRepository.Delete(tutorSkill);
        }

        public List<Tutor> GetBySkill(LanguageLevel languageLevel)
        {
            return tutorSkillRepository.GetBySkill(languageLevel);
        }
    }
}
