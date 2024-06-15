using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface ICourseTimeSlotRepository
    {
        public List<CourseTimeSlot> GetAll();
        public CourseTimeSlot Get(int id);
        public void Add(CourseTimeSlot courseTimeSlot);
        public List<TimeSlot> GetByCourse(Course course);
    }
}
