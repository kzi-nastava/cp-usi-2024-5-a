using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories
{
    public class LanguageLevelRepository : ILanguageLevelRepository
    {
        private readonly DatabaseContext _context;

        public LanguageLevelRepository(DatabaseContext context)
        {
            _context = context;
        }
        public void Add(LanguageLevel language)
        {
            _context.LanguageLevel.Add(language);
            _context.SaveChanges();
        }

        public LanguageLevel Get(int id)
        {
            return _context.LanguageLevel.Find(id);
        }

        public List<LanguageLevel> GetAll()
        {
            return _context.LanguageLevel.ToList();
        }
    }
}
