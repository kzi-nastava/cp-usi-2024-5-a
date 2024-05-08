using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LangLang.Core.Repository.Serialization;

namespace LangLang.Core.Model
{
    public class Course : ISerializable
    {
        // Properties
        public int Id { get; set; }
        public int TutorId { get; set; }
        public string Language { get; set; }
        public LanguageLevel Level { get; set; }
        public int NumberOfWeeks { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public bool Online { get; set; }
        public int NumberOfStudents { get; set; }
        public int MaxStudents { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool CreatedByDirector { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }
        public bool Modifiable { get; set; }


        // Constructors

        public Course(int id, int tutorId, string language, LanguageLevel level, int numberOfWeeks, List<DayOfWeek> days,
            bool online, int numberOfStudents, int maxStudents, DateTime startDateTime, bool createdByDirector, bool modifiable)
        {
            Id = id;
            TutorId = tutorId;
            Language = language;
            Level = level;
            NumberOfWeeks = numberOfWeeks;
            Days = days;
            Online = online;
            NumberOfStudents = numberOfStudents;
            MaxStudents = maxStudents;
            StartDateTime = startDateTime;
            CreatedByDirector = createdByDirector;
            Modifiable = modifiable;
            GenerateTimeSlots();
        }
        
        public Course()
        { 
        }

        public string ToString()
        {
            StringBuilder sbDays = new StringBuilder();
            foreach (DayOfWeek day in Days)
            {
                sbDays.Append(day.ToString() + " ");
            }

            // Deletes the last white space from stringbuilder
            if (sbDays.Length > 0)
            {
                sbDays.Remove(sbDays.Length - 1, 1);
            }

            return $"ID: {Id,5} | TutorId: {TutorId,5} | Language: {Language,20} | Level: {Level,5} | NumberOfWeeks: {NumberOfWeeks,5} | Days: {sbDays, 10} | Online: {Online,5} | NumberOfStudents : {NumberOfStudents,5} | MaxStudents : {MaxStudents,5} | StartDateTime : {StartDateTime,10} | CreatedByDirector : {CreatedByDirector,5} | Modifiable : {Modifiable, 5} |";
        }

        public string[] ToCSV()
        {
            StringBuilder sbDays = new StringBuilder();
            foreach (DayOfWeek day in Days)
            {
                sbDays.Append(day.ToString() + " ");
            }

            // Deletes the last white space from stringbuilder
            if (sbDays.Length > 0)
            {
                sbDays.Remove(sbDays.Length - 1, 1);
            }

            string[] csvValues =
            {
                Id.ToString(),
                TutorId.ToString(),
                Language,
                Level.ToString(),
                NumberOfWeeks.ToString(),
                sbDays.ToString(),
                Online.ToString(),
                NumberOfStudents.ToString(),
                MaxStudents.ToString(),
                StartDateTime.ToString(),
                CreatedByDirector.ToString(),
                Modifiable.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TutorId = int.Parse(values[1]);
            Language = values[2];
            Level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), values[3]);
            NumberOfWeeks = int.Parse(values[4]);

            // Converting from string to list of WeekDays
            string[] days = values[5].Split(' ');
            Days = new List<DayOfWeek>();
            foreach (string day in days)
            {
                Days.Add((DayOfWeek)Enum.Parse(typeof(DayOfWeek), day));
            }

            Online = bool.Parse(values[6]);
            NumberOfStudents = int.Parse(values[7]);
            MaxStudents = int.Parse(values[8]);
            StartDateTime = DateTime.Parse(values[9]);
            CreatedByDirector = bool.Parse(values[10]);
            Modifiable = bool.Parse(values[11]);
            GenerateTimeSlots();
        }

        public bool IsCompleted()
        {
            TimeSlot timeSlot = TimeSlots[TimeSlots.Count - 1];
            return DateTime.Now >= timeSlot.GetEnd();
        }

        // this method generates all timeslots for a course based on number of weeks, days and start datetime
        private void GenerateTimeSlots()
        {
            TimeSlots = new List<TimeSlot>();
            int skipToNextWeek = 0;
            for (int week = 0; week < NumberOfWeeks; week++)
            {
                foreach (DayOfWeek day in Days)
                { 
                    // if the start date is after some of the days,
                    // skipped over to them in next week
                    if (day - StartDateTime.DayOfWeek < 0) 
                    {
                        skipToNextWeek = 7;
                    }
                    else
                    {
                        skipToNextWeek = 0;
                    }
                    DateTime classDate = StartDateTime.AddDays(week * 7 + (skipToNextWeek + day - StartDateTime.DayOfWeek));
                    TimeSlots.Add(new TimeSlot(Constants.SESSION_DURATION, classDate));
                }
            }
        }

        // this method checks if timeSlot overlapps with any of the course's timeslots
        public bool OverlappsWith(TimeSlot timeSlot)
        {
            foreach (TimeSlot time in TimeSlots)
            {
                if (time.OverlappsWith(timeSlot))
                {
                    return true;
                }
            }
            return false;
        }
        public int DaysUntilEnd()
        {
            var endDate = TimeSlots[TimeSlots.Count - 1].GetEnd();
            return (endDate - DateTime.Now).Days;
        }

        public int DaysUntilStart()
        {
            return (StartDateTime - DateTime.Now).Days;
        }
    }
}
