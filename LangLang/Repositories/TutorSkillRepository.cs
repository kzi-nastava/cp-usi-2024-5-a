using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories
{
    internal class TutorSkillRepository : ITutorSkillRepository
    {
        private readonly DatabaseContext _databaseContext;

        public TutorSkillRepository(DatabaseContext context)
        {
            _databaseContext = context;
        }

        public int Add(TutorSkill skillTutor)
        {
            _databaseContext.Add(skillTutor);
            _databaseContext.SaveChanges();
            return skillTutor.Id;
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

        public List<LanguageLevel> GetByTutor(Tutor tutor)
        {
            return (from ts in _databaseContext.TutorSkill
                    join ll in _databaseContext.LanguageLevel on ts.LanguageLevelId equals ll.Id
                    where ts.TutorId == tutor.Id
                    select ll).Distinct().ToList();
        }

        public bool AlreadyAdded(int tutorId, int skillId)
        {
            return _databaseContext.TutorSkill.Any(ts => ts.TutorId == tutorId && ts.LanguageLevelId == skillId);
        }

    }
}
