using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LangLang.Core.Model
{
    class Course
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

        public Course(string language, LanguageLevel level, int numberOfWeeks, List<WeekDays> days, bool online, int numberOfStudents, int maxStudents)
        {
            Language = language;
            Level = level;
            NumberOfWeeks = numberOfWeeks;
            Days = days;
            Online = online;
            NumberOfStudents = numberOfStudents;
            MaxStudents = maxStudents;
            CreationDate = DateTime.Now;
        }

        public Course()
        { 
        }
    }
}
