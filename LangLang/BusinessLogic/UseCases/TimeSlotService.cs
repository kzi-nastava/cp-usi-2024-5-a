using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.BusinessLogic.UseCases
{
    public class TimeSlotService
    {
        private ITimeSlotRepository _repository;

        public TimeSlotService()
        {
            _repository = Injector.CreateInstance<ITimeSlotRepository>();
        }

        public List<TimeSlot> GetAll()
        {
            return _repository.GetAll();
        }

        public TimeSlot Get(int id)
        {
            return _repository.Get(id);
        }

        public TimeSlot Add(TimeSlot timeslot)
        {
            _repository.Add(timeslot);
            return timeslot;
        }

    }
}
