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
        // Attributes
        private int _id;
        private int _tutorId;
        private string _language;
        private LanguageLevel _level;
        private int _numberOfWeeks;
        private List<DayOfWeek> _days;
        private bool _online;
        private int _numberOfStudents;
        private int _maxStudents;
        private DateTime _startDateTime;

        // Properties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int TutorId
        {
            get { return _tutorId; }
            set { _tutorId = value; }
        }

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public LanguageLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }
     
        public int NumberOfWeeks
        {
            get { return _numberOfWeeks; }
            set { _numberOfWeeks = value; }
        }
        public List<DayOfWeek> Days
        {
            get { return _days; }
            set { _days = value; }
        }
        
        public bool Online
        {
            get { return _online; }
            set { _online = value; }
        }

        public int NumberOfStudents
        {
            get { return _numberOfStudents; }
            set { _numberOfStudents = value; }
        }

        public int MaxStudents
        {
            get { return _maxStudents; }
            set { _maxStudents = value; }
        }

        public DateTime StartDateTime 
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }

        // Constructors

        public Course(int id, int tutorId, string language, LanguageLevel level, int numberOfWeeks, List<DayOfWeek> days,
            bool online, int maxStudents, DateTime startDateTime)
        {
            Id = id;
            TutorId = tutorId;
            Language = language;
            Level = level;
            NumberOfWeeks = numberOfWeeks;
            Days = days;
            Online = online;
            NumberOfStudents = 0;
            MaxStudents = maxStudents;
            StartDateTime = startDateTime;
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

            return $"ID: {Id,5} | Language: {Language,20} | Level: {Level,5} | NumberOfWeeks: {NumberOfWeeks,5} | Days: {sbDays, 10} | Online: {Online,5} | NumberOfStudents : {NumberOfStudents,5} | MaxStudents : {MaxStudents,5} | StartDateTime : {StartDateTime,10} |";
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
                Language,
                Level.ToString(),
                NumberOfWeeks.ToString(),
                sbDays.ToString(),
                Online.ToString(),
                NumberOfStudents.ToString(),
                MaxStudents.ToString(),
                StartDateTime.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Language = values[1];
            Level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), values[2]);
            NumberOfWeeks = int.Parse(values[3]);

            // Converting from string to list of WeekDays
            string[] days = values[4].Split(' ');
            Days = new List<DayOfWeek>();
            foreach (string day in days)
            {
                Days.Add((DayOfWeek)Enum.Parse(typeof(DayOfWeek), day));
            }

            Online = bool.Parse(values[5]);
            NumberOfStudents = int.Parse(values[6]);
            MaxStudents = int.Parse(values[7]);
            StartDateTime = DateTime.Parse(values[8]);
        }
    }
}
