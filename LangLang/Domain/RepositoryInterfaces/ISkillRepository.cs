using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface ISkillRepository
    {
        public List<Skill> GetAll();
        public Skill Get(int id);
        public void Add(Skill skill);
    }
}
