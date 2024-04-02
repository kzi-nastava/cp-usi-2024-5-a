using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Core.Model;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Reflection;
using System.Windows;


namespace LangLang.DTO
{
    public class CourseDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        public string StringDays { get; }

        public int Id { get; set; }
        private int tutorId;
        private string language;
        private LanguageLevel level;
        private int numberOfWeeks;
        private List<DayOfWeek> days;
        private bool online;
        private int maxStudents;
        private DateTime startDateTime;
        private bool createdByDirector;
        private string time;
        private bool mon;
        private bool tue;
        private bool wed;
        private bool thu;
        private bool fri;

        public bool Mon
        {
            get
            {
                return mon;
            }
            set
            {
                if (value != mon)
                {
                    mon = value;
                    OnPropertyChanged("Mon");
                }
            }
        }
        public bool Tue
        {
            get
            {
                return tue;
            }
            set
            {
                if (value != tue)
                {
                    tue = value;
                    OnPropertyChanged("Tue");
                }
            }
        }
        public bool Wed
        {
            get
            {
                return wed;
            }
            set
            {
                if (value != wed)
                {
                    wed = value;
                    OnPropertyChanged("Wed");
                }
            }
        }

        public bool Thu
        {
            get
            {
                return thu;
            }
            set
            {
                if (value != thu)
                {
                    thu = value;
                    OnPropertyChanged("Thu");
                }
            }
        }
        public bool Fri
        {
            get
            {
                return fri;
            }
            set
            {
                if (value != fri)
                {
                    fri = value;
                    OnPropertyChanged("Fri");
                }
            }
        }
        public string Language
        {
            get
            {
                return language;
            }
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                if (value != time)
                {
                    time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                return startDateTime;
            }
            set
            {
                if (value != startDateTime)
                {
                    startDateTime = value;
                    OnPropertyChanged("StartDateTime");
                }
            }
        }

        public List<DayOfWeek> Days
        {
            get
            {
                return days;
            }
            set
            {
                if (value != days)
                {
                    days = value;
                    OnPropertyChanged("Days");
                }
            }
        }

        public LanguageLevel Level
        {
            get
            {
                return level;
            }
            set
            {
                if (value != level)
                {
                    level = value;
                    OnPropertyChanged("Level");
                }
            }
        }

        public string NumberOfWeeks
        {
            get
            {
                return numberOfWeeks.ToString();
            }
            set
            {
                if (int.TryParse(value, out int result) && result >= 0)
                {
                    numberOfWeeks = result;
                }
                else
                {
                    numberOfWeeks = 0;
                }
            }
        }

        public string MaxStudents
        {
            get { return maxStudents.ToString(); }
            set
            {
                if (int.TryParse(value, out int result) && result >= 0)
                {
                    maxStudents = result;
                }
                else
                {
                    maxStudents = 0;
                }
            }
        }

        public int TutorId
        {
            get
            {
                return tutorId;
            }
            set
            {
                if (value != tutorId)
                {
                    tutorId = value;
                    OnPropertyChanged("TutorId");
                }
            }
        }

        public bool NotOnline
        {
            get
            {
                return !online;
            }
            set
            {
                if (value == online)
                {
                    online = !value;
                    OnPropertyChanged("Online");
                }
            }
        }

        public bool CreatedByDirector
        {
            get
            {
                return online;
            }
            set
            {
                if (value != createdByDirector)
                {
                    createdByDirector = value;
                    OnPropertyChanged("CreatedByDirector");
                }
            }
        }

        private readonly Regex _TimeRegex = new("^([01]?[0-9]|2[0-3]):[0-5][0-9]$");


        public string this[string columnName]
        {
            get
            {
                if (columnName == "Language")
                {
                    if (string.IsNullOrEmpty(Language)) return "Language is required";
                    else return "";
                }
                if (columnName == "Time")
                {
                    if (string.IsNullOrEmpty(Time)) return "Time is required";
                    if (!_TimeRegex.Match(time).Success) return "Time format must be HH:mm";
                    string[] timeParts = time.Split(':');
                    if (timeParts.Length != 2) return "Time format must be HH:mm";
                    else return "";
                }
                if (columnName == "StartDateTime")
                {
                    if (startDateTime < DateTime.Now) return "Please enter a valid date. Dates in the past are not allowed.";
                    if (startDateTime == default) return "Birth date is required";
                    else return "";
                }
                if (columnName == "NumberOfWeeks")
                {
                    if (!int.TryParse(NumberOfWeeks, out int _numberOfWeeks) || _numberOfWeeks <= 0) return "Number of weeks must be a positive number";
                    else return "";
                }
                if (columnName == "MaxStudents")
                {
                    if (!int.TryParse(MaxStudents, out int _maxStudents) || _maxStudents <= 0) return "Maximal number of students must be a positive number";
                    else return "";
                }
                return "";
            }
        }
        public string ConcatenatedDays
        {
            get { return string.Join(", ", Days); }
        }

        private string[] _validatedProperties = { "StartDateTime", "Language", "Level", "NumberOfWeeks", "Time" };

        // checks if all properties are valid
        public bool IsValid
        {
            get
            {
                if (online == false)
                {
                    _validatedProperties = _validatedProperties.Append("MaxStudents").ToArray();
                }
                else
                {
                    int index = Array.IndexOf(_validatedProperties, "MaxStudents");
                    if (index != -1)
                    {
                        // Create a new array with one less element
                        string[] newArray = new string[_validatedProperties.Length - 1];

                        // Copy elements from the original array to the new array, excluding the element to delete
                        Array.Copy(_validatedProperties, 0, newArray, 0, index);
                        Array.Copy(_validatedProperties, index + 1, newArray, index, _validatedProperties.Length - index - 1);

                        // Replace the original array with the new array
                        _validatedProperties = newArray;
                    }
                }
                days = new List<DayOfWeek>();
                if (mon)
                {
                    days.Add(DayOfWeek.Monday);
                }
                if (tue)
                {
                    days.Add(DayOfWeek.Tuesday);
                }
                if (wed)
                {
                    days.Add(DayOfWeek.Wednesday);
                }
                if (thu)
                {
                    days.Add(DayOfWeek.Thursday);
                }
                if (fri)
                {
                    days.Add(DayOfWeek.Friday);
                }
                if (days.Count == 0)
                {
                    return false;
                }
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != "")
                        return false;
                }

                return true;
            }
        }

        public string Error => null;

        public CourseDTO()
        {
            days = new List<DayOfWeek>();
            online = true;
            mon = false;
            tue = false;
            fri = false;
            wed = false;
            thu = false;
        }

        public Course ToCourse()
        {
            string[] timeParts = time.Split(':');
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            return new Course(Id, tutorId, language, level, numberOfWeeks, days, online, maxStudents, new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, hour, minute, 0), createdByDirector);
        }

        public CourseDTO(Course course)
        {
            this.Id = course.Id;
            Language = course.Language;
            Level = course.Level;
            NotOnline = !course.Online;
            CreatedByDirector = course.CreatedByDirector;
            TutorId = course.TutorId;
            Days = course.Days;
            StringBuilder sbDays = new StringBuilder(); 
            if (Days.Contains(DayOfWeek.Monday))
            {
                Mon = true;
                sbDays.Append("Mon ");
            }
            else
            {
                Mon = false;
            }
            if (Days.Contains(DayOfWeek.Tuesday))
            {
                Tue = true;
                sbDays.Append("Tue ");
            }
            else
            {
                Tue = false;
            }
            if (Days.Contains(DayOfWeek.Wednesday))
            {
                Wed = true;
                sbDays.Append("Wed ");
            }
            else
            {
                Wed = false;
            }
            if (Days.Contains(DayOfWeek.Thursday))
            {
                Thu = true;
                sbDays.Append("Thu ");
            }
            else
            {
                Thu = false;
            }
            if (Days.Contains(DayOfWeek.Friday))
            {
                Fri = true;
                sbDays.Append("Fri ");
            }
            else
            {
                Fri = false;
            }
            // Deletes the last white space from stringbuilder
            if (sbDays.Length > 0)
            {
                sbDays.Remove(sbDays.Length - 1, 1);
            }
            StringDays = sbDays.ToString();
            StartDateTime = course.StartDateTime;
            NumberOfWeeks = course.NumberOfWeeks.ToString();
            MaxStudents = course.MaxStudents.ToString();
            Time = course.StartDateTime.ToString("HH:mm");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
