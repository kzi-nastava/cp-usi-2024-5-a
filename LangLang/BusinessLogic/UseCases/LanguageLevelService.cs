using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.BusinessLogic.UseCases
{
    public class LanguageLevelService
    {
        private ILanguageLevelRepository _language;

        public LanguageLevelService() {
            _language = Injector.CreateInstance<ILanguageLevelRepository>();
        }

        public int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public List<LanguageLevel> GetAll()
        {
            return _language.GetAll();
        }

        public void Add(LanguageLevel language)
        {
            language.Id = GenerateId();
            _language.Add(language);
        }

        public LanguageLevel Get(int id)
        {
            return _language.Get(id);
        }
    }
}
