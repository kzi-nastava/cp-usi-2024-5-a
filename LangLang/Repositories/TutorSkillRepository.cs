using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LangLang.Repositories
{
    internal class TutorSkillRepository : ITutorSkillRepository
    {
        private readonly DatabaseContext _databaseContext;
        public void Add(TutorSkill skillTutor)
        {
            _databaseContext.Add(skillTutor);
            _databaseContext.SaveChanges();
        }

        public void Delete(TutorSkill skillTutor)
        {
            _databaseContext.Remove(skillTutor);
            _databaseContext.SaveChanges();
        }

        public TutorSkill Get(int id)
        {
            return _databaseContext.TutorSkill.Find(id);
        }

        public List<TutorSkill> GetAll()
        {
            return _databaseContext.TutorSkill.ToList();
        }

        public List<Tutor> GetBySkill(LanguageLevel skill)
        {
            return (from ts in _databaseContext.TutorSkill
                    join t in _databaseContext.Tutor on ts.TutorId equals t.Id
                    join ll in _databaseContext.LanguageLevel on ts.LanguageLevelId equals ll.Id
                    where ll.Language == skill.Language && ll.Level == skill.Level
                    select t).Distinct().ToList();
        }
    }
}
