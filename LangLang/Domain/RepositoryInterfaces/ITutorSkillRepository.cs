
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface ITutorSkillRepository
    {
        public List<TutorSkill> GetAll();
        public TutorSkill Get(int id);
        public void Add(TutorSkill skillTutor);
        public void Delete(TutorSkill skillTutor);
        public List<Tutor> GetBySkill(LanguageLevel skill);

    }
}
