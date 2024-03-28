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
        private string language;
        private LanguageLevel level;
        private int numberOfWeeks;
        private List<WeekDays> days;
        private bool online;
        private int numberOfStudents;
        private int maxStudents;

        // Properties
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public LanguageLevel Level
        {
            get { return level; }
            set { level = value; }
        }
     
        public int NumberOfWeeks
        {
            get { return numberOfWeeks; }
            set { numberOfWeeks = value; }
        }
        public List<WeekDays> Days
        {
            get { return days; }
            set { days = value; }
        }
        
        public bool Online
        {
            get { return online; }
            set { online = value; }
        }

        public int NumberOfStudents
        {
            get { return numberOfStudents; }
            set { numberOfStudents = value; }
        }

        public int MaxStudents
        {
            get { return maxStudents; }
            set { maxStudents = value; }
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
        }

        public Course()
        { 
        }
    }
}
