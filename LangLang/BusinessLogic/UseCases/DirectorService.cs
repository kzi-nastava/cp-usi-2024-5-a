﻿using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;

namespace LangLang.BusinessLogic.UseCases
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

        public Director Get(int id)
        {
            return _directors.Get(id);
        }
    }
}
