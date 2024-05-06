using LangLang.Core.Repository;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Model.DAO
{
    public class DirectorDAO
    {
        private readonly Dictionary<int, Director> _directors;
        private readonly Repository<Director> _repository;

        public DirectorDAO()
        {
            _repository = new Repository<Director>("directors.csv");
            _directors = _repository.Load();
        }

        public List<Director> GetAll()
        {
            return _directors.Values.ToList();
        }

    }
}
