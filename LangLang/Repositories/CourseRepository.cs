using LangLang.Configuration;
using LangLang.Core.Observer;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repositories
{
    public class CourseRepository :Subject, ICourseRepository
    {
        private readonly Dictionary<int, Course> _courses;
        private string _filePath = Constants.FILENAME_PREFIX + "courses.csv";
        public CourseRepository()
        {
            _courses = Load();
        }
        public Course Get(int id)
        {
            return _courses[id];
        }
        public List<Course> GetAll()
        {
            return _courses.Values.ToList();    
        }

        public void Save()
        {
            using var writer = new StreamWriter(_filePath);

            foreach (var course in GetAll())
            {
                var line = course.ToString();
                writer.WriteLine(line);
            }
        }

        public Dictionary<int, Course> Load()
        {
            var courses = new Dictionary<int, Course>();

            if (!File.Exists(_filePath)) return courses;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var tokens = line.Split(Constants.DELIMITER);

                int id = int.Parse(tokens[0]);
                int tutorId = int.Parse(tokens[1]);
                var language = tokens[2];
                var level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), tokens[3]);
                int numOfWeeks = int.Parse(tokens[4]);

                // Converting from string to list of WeekDays
                string[] days = tokens[5].Split(' ');
                var daysOfWeek = new List<DayOfWeek>();
                foreach (string day in days)
                {
                    daysOfWeek.Add((DayOfWeek)Enum.Parse(typeof(DayOfWeek), day));
                }

                bool online = bool.Parse(tokens[6]);
                int numOfStud = int.Parse(tokens[7]);
                int maxStud = int.Parse(tokens[8]);
                var startDateTime = DateTime.Parse(tokens[9]);
                bool createdByDirector = bool.Parse(tokens[10]);
                bool modifiable = bool.Parse(tokens[11]);

                var course = new Course(id, tutorId, language, level, numOfWeeks, daysOfWeek, online, numOfStud, maxStud, startDateTime, createdByDirector, modifiable);

                courses.Add(course.Id, course);
            }

            return courses;
        }

        public void Add(Course course)
        {
            _courses.Add(course.Id, course);
            Save();
            NotifyObservers();
        }

        public void Update(Course course)
        {
            Course oldCourse = Get(course.Id);
            if (oldCourse == null) return;

            oldCourse.Language = course.Language;
            oldCourse.Level = course.Level;
            oldCourse.NumberOfWeeks = course.NumberOfWeeks;
            oldCourse.Days = course.Days;
            oldCourse.Online = course.Online;
            oldCourse.NumberOfStudents = course.NumberOfStudents;
            oldCourse.MaxStudents = course.MaxStudents;
            oldCourse.StartDateTime = course.StartDateTime;
            oldCourse.TutorId = course.TutorId;
            oldCourse.Modifiable = course.Modifiable;

            Save();
            NotifyObservers();
        }

        public void Delete(int id)
        {
            _courses.Remove(id);
            Save();
            NotifyObservers();
        }

    }
}
