using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;

namespace LangLang.Aplication.UseCases
{
    public class DirectorService
    {
        private IDirectorRepository _directors;
        public DirectorService() { 
            _directors = Injector.CreateInstance<IDirectorRepository>();
        }

        public List<Director> GetAll()
        {
            return _directors.GetAll();
        }
    }
}
