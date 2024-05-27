
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface ITutorRatingRepository
    {
        public List<TutorRating> GetAll();
        public TutorRating Get(int id);
        public void Add(TutorRating rating);
        public void Save();
        public Dictionary<int, TutorRating> Load();
    }
}
