using LangLang.Core.Observer;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Domain.RepositoryInterfaces
{
    internal interface IGradeRepository
    {
        public List<Grade> GetAll();
        public Grade Get(int id);
        public void Add(Grade grade);
        public void Update(Grade grade);
        public void Delete(int id);
        public void Save();
        public Dictionary<int, Grade> Load();
    }
}
