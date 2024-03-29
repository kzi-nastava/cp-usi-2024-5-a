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
        private string _language;
        private LanguageLevel _level;
        private int _numberOfWeeks;
        private List<WeekDays> _days;
        private bool _online;
        private int _numberOfStudents;
        private int _maxStudents;
        private DateTime _creationDate;

        // Properties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
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
        public List<WeekDays> Days
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

        public DateTime CreationDate 
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        // Constructors

        public Course(int id, string language, LanguageLevel level, int numberOfWeeks, List<WeekDays> days, bool online, int maxStudents)
        {
            Id = id;
            Language = language;
            Level = level;
            NumberOfWeeks = numberOfWeeks;
            Days = days;
            Online = online;
            NumberOfStudents = 0;
            MaxStudents = maxStudents;
            CreationDate = DateTime.Now;
        }

        public Course()
        { 
        }

        public override string ToString()
        {
            StringBuilder sbDays = new StringBuilder();
            foreach (WeekDays day in Days)
            {
                sbDays.Append(day.ToString() + " ");
            }

            // Deletes the last white space from stringbuilder
            if (sbDays.Length > 0)
            {
                sbDays.Remove(sbDays.Length - 1, 1);
            }

            return $"ID: {Id,5} | Language: {Language,20} | Level: {Level,5} | NumberOfWeeks: {NumberOfWeeks,5} | Days: {sbDays, 10} | Online: {Online,5} | NumberOfStudents : {NumberOfStudents,5} | MaxStudents : {MaxStudents,5} | CreationDate : {CreationDate,10} |";
        }

        public string[] ToCSV()
        {
            StringBuilder sbDays = new StringBuilder();
            foreach (WeekDays day in Days)
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
                CreationDate.ToString()
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
            Days = new List<WeekDays>();
            foreach (string day in days)
            {
                Days.Add((WeekDays)Enum.Parse(typeof(WeekDays), day));
            }

            Online = bool.Parse(values[5]);
            NumberOfStudents = int.Parse(values[6]);
            MaxStudents = int.Parse(values[7]);
            CreationDate = DateTime.Parse(values[8]);
        }
    }
}
