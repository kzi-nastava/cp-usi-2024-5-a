using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface ITimeSlotRepository
    {
        public List<TimeSlot> GetAll();
        public TimeSlot Get(int id);
        public void Add(TimeSlot timeSlot);
    }
}