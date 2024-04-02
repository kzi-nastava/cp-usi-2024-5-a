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
        private bool _createdByDirector;

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

        public bool CreatedByDirector
        {
            get { return _createdByDirector; }
            set { _createdByDirector = value; }
        }

        // Constructors

        public Course(int id, int tutorId, string language, LanguageLevel level, int numberOfWeeks, List<DayOfWeek> days,
            bool online, int maxStudents, DateTime startDateTime, bool createdByDirector)
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
            CreatedByDirector = createdByDirector;
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

            return $"ID: {Id,5} | TutorId: {TutorId,5} | Language: {Language,20} | Level: {Level,5} | NumberOfWeeks: {NumberOfWeeks,5} | Days: {sbDays, 10} | Online: {Online,5} | NumberOfStudents : {NumberOfStudents,5} | MaxStudents : {MaxStudents,5} | StartDateTime : {StartDateTime,10} | CreatedByDirector : {CreatedByDirector,5} |";
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
                CreatedByDirector.ToString()
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
        }
    }
}
