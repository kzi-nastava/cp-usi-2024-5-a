using System.Collections.Generic;
using LangLang.Domain.Models;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IDirectorRepository{
        public List<Director> GetAll();
        public void Save();
        public Dictionary<int, Director> Load();
    }
}
