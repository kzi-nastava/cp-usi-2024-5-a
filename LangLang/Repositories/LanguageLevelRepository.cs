using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
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
        public int Add(LanguageLevel language)
        {
            var existingLanguageLevel = GetExistingLanguageLevel(language);

            if (existingLanguageLevel != null)
            {
                return existingLanguageLevel.Id;
            }
            return AddNewLanguageLevel(language);
        }

        private LanguageLevel? GetExistingLanguageLevel(LanguageLevel language)
        {
            return Get(language.Language, language.Level);
        }

        private int AddNewLanguageLevel(LanguageLevel language)
        {
            _context.LanguageLevel.Add(language);
            _context.SaveChanges();
            return language.Id;
        }

        public LanguageLevel? Get(int id)
        {
            return _context.LanguageLevel.Find(id);
        }

        public List<LanguageLevel> GetAll()
        {
            return _context.LanguageLevel.ToList();
        }

        private LanguageLevel? Get(string language, Level level)
        {
            return _context.LanguageLevel
                .FirstOrDefault(ll => ll.Language.ToLower() == language.ToLower() && ll.Level == level);
        }


    }
}
