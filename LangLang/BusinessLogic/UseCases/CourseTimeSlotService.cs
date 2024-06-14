using LangLang.Composition;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.BusinessLogic.UseCases
{
    public class CourseTimeSlotService
    {
        public ICourseTimeSlotRepository _repository;
        
        public CourseTimeSlotService()
        {
            _repository = Injector.CreateInstance<ICourseTimeSlotRepository>();
        }

        public void Add(CourseTimeSlot courseTimeSlot)
        {
            _repository.Add(courseTimeSlot);
        }
        
        public List<CourseTimeSlot> GetAll()
        {
            return _repository.GetAll();
        }

        public CourseTimeSlot Get(int id)
        {
            return _repository.Get(id);
        }

    }
}
