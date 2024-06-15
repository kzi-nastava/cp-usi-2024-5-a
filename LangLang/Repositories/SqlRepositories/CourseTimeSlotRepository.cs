using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Repositories.SqlRepositories
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

        public List<TimeSlot> GetByCourse(Course course)
        {
            var timeSlots = _context.CourseTimeSlot
                                    .Where(cts => cts.CourseId == course.Id)
                                    .Join(_context.TimeSlot,
                                          cts => cts.TimeSlotId,
                                          ts => ts.Id,
                                          (cts, ts) => ts)
                                    .ToList();
            return timeSlots;
        }

    }
}
