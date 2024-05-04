
using LangLang.Core.Model.DAO;
using LangLang.Core.Model;
using System.Collections.Generic;

namespace LangLang.Core.Controller
{
    public class DirectorController
    {
        private readonly DirectorDAO _directors;

        public DirectorController()
        {
            _directors = new DirectorDAO();
        }

        public List<Director> GetAllDirectors()
        {
            return _directors.GetAllDirectors();
        }
    }
}
