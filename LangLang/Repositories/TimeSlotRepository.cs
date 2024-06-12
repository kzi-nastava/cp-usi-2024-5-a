using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly DatabaseContext _databaseContext;

        public TimeSlotRepository(DatabaseContext context) { 
            _databaseContext = context;
        }

        public void Add(TimeSlot timeSlot)
        {
            _databaseContext.TimeSlot.Add(timeSlot);
            _databaseContext.SaveChanges();
        }

        public TimeSlot Get(int id)
        {
            return _databaseContext.TimeSlot.Find(id);
        }

        public List<TimeSlot> GetAll()
        {
            return _databaseContext.TimeSlot.ToList();
        }
    }
}
