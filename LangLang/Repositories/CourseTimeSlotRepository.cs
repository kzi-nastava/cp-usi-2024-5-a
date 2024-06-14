using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories
{
    public class CourseTimeSlotRepository : ICourseTimeSlotRepository
    {
        public DatabaseContext _context;

        public CourseTimeSlotRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(CourseTimeSlot courseTimeSlot)
        {
            _context.CourseTimeSlot.Add(courseTimeSlot);
            _context.SaveChanges();
        }

        public CourseTimeSlot Get(int id)
        {
            return _context.CourseTimeSlot.Find(id);
        }

        public List<CourseTimeSlot> GetAll()
        {
            return _context.CourseTimeSlot.ToList();
        }

    }
}
