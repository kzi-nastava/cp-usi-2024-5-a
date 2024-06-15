using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;

namespace LangLang.BusinessLogic.UseCases
{
    public class LanguageLevelService
    {
        private ILanguageLevelRepository _language;

        public LanguageLevelService() {
            _language = Injector.CreateInstance<ILanguageLevelRepository>();
        }

        public List<LanguageLevel> GetAll()
        {
            return _language.GetAll();
        }

        public int Add(LanguageLevel language)
        {
            return _language.Add(language);
        }

        public LanguageLevel Get(int id)
        {
            return _language.Get(id);
        }
    }
}
