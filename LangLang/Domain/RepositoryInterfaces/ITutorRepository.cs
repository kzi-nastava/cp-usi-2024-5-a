using LangLang.Core.Observer;
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface ITutorRepository
    {
        public List<Tutor> GetAll();
        public Tutor Get(int id);
        public void Add(Tutor tutor);
        public void Update(Tutor tutor);
        public void Deactivate(int id);
        public void Save();
        public Dictionary<int, Tutor> Load();
        public void Subscribe(IObserver observer);
    }
}
