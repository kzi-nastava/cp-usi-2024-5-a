

using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.BusinessLogic.UseCases
{
    public class SkillService
    {
        private ISkillRepository _skills;

        public SkillService() { }

        public int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public List<Skill> GetAll()
        {
            return _skills.GetAll();
        }

        public void Add(Skill skill)
        {
            skill.Id = GenerateId();
            _skills.Add(skill);
        }

        public Skill Get(int id)
        {
            return _skills.Get(id);
        }
    }
}
