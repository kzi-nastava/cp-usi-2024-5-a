using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface ILanguageLevelRepository
    {
        public List<LanguageLevel> GetAll();
        public LanguageLevel? Get(int id);
        public int Add(LanguageLevel language);
    }
}
