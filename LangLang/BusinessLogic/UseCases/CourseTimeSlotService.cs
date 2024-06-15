using LangLang.Composition;
using LangLang.Configuration;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<TimeSlot> GetByCourse(Course course)
        {
            return _repository.GetByCourse(course);
        }

        public List<TimeSlot> GetSortedByEndTime(Course course)
        {
            var timeSlots = GetByCourse(course);
            return timeSlots.OrderBy(ts => ts.GetEnd()).ToList();
        }

        public void GenerateSlots(Course course)
        {
            var timeService = new TimeSlotService();
            DateTime startDateTime = course.StartDateTime;
            DayOfWeek startDayOfWeek = startDateTime.DayOfWeek;

            for (int i = 0; i < course.NumberOfWeeks; ++i)
            {
                foreach (DayOfWeek day in course.Days)
                {
                    int skipToNextWeek = (day < startDayOfWeek) ? 7 : 0;
                    DateTime classDate = startDateTime.AddDays(i * 7 + skipToNextWeek + (day - startDayOfWeek));

                    var timeSlot = timeService.Add(new TimeSlot(Constants.SESSION_DURATION, classDate));
                    Add(new CourseTimeSlot(course.Id, timeSlot.Id));
                }
            }
        }

    }
}
