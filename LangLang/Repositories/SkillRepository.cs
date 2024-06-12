
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly DatabaseContext _databaseContext;

        public SkillRepository(DatabaseContext context)
        {
            _databaseContext = context;
        }
        public void Add(Skill skill)
        {
            _databaseContext.Skill.Add(skill);
            _databaseContext.SaveChanges();
        }

        public Skill Get(int id)
        {
            return _databaseContext.Skill.Find(id);
        }

        public List<Skill> GetAll()
        {
            return _databaseContext.Skill.ToList();
        }
    }
}
