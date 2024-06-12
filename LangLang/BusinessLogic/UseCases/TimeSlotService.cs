using LangLang.Composition;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.BusinessLogic.UseCases
{
    public class TimeSlotService
    {


        private ITimeSlotRepository _timeSlot;


        public TimeSlotService()
        {
            _timeSlot = Injector.CreateInstance<ITimeSlotRepository>();
        }

        private int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public List<TimeSlot> GetAll()
        {
            return _timeSlot.GetAll();
        }

        public TimeSlot Get(int id)
        {
            return _timeSlot.Get(id);
        }

        public void Add(TimeSlot timeslot)
        {
            timeslot.Id = GenerateId();
            _timeSlot.Add(timeslot);
        }

    }
}
