using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.Domain.Enums;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CourseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public string StringDays { get; set; }

        public int Id { get; set; }
        public int TutorId { get; set; }
        public string TutorFullName { get; set; }
        public bool Modifiable { get; set; }
        public int NumberOfStudents { get; set; }
        public string DaysUntilEnd { get; set; }
        public bool CreatedByDirector { get; set; }
        public int GradedStudentsCount {  get; set; }

        public bool GratitudeEmailSent {  get; set; }

        private string language;
        private LanguageLevel level;
        private int numberOfWeeks;
        private bool online;
        private int maxStudents;
        private DateTime startDate;
        private string time;
        private List<bool> booleanDays;

        public List<bool> BooleanDays
        {
            get => booleanDays;
            set
            {
                booleanDays = value;
                OnPropertyChanged(nameof(BooleanDays));
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

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged("StartDate");
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
                if (columnName == "StartDate")
                {
                    if (startDate < DateTime.Now) return "Please enter a valid date. Dates in the past are not allowed.";
                    if (startDate == default) return "Birth date is required";
                    else return "";
                }
                if (columnName == "NumberOfWeeks")
                {
                    if (!int.TryParse(NumberOfWeeks, out int _numberOfWeeks) || _numberOfWeeks <= 0) return "Number of weeks must be a positive number";
                    else return "";
                }
                if (columnName == "MaxStudents")
                {
                    if (online) return "";
                    if (!int.TryParse(MaxStudents, out int _maxStudents) || _maxStudents <= 0) return "Maximal number of students must be a positive number";
                    else return "";
                }
                return "";
            }
        }

        private string[] _validatedProperties = { "NotOnline", "MaxStudents", "StartDate", "Language", "Level", "NumberOfWeeks", "Time" };

        // checks if all properties are valid
        public bool IsValid
        {
            get
            {

                foreach (var property in _validatedProperties)
                {
                    if (this[property] != "") return false;
                }

                //check whether at least one day is selected
                for (int i = 0; i < 5; i++)
                {
                    if (BooleanDays[i]) return true;
                }

                return false;
            }
        }

        public string Error => null;

        public CourseViewModel()
        {
            online = true;
            BooleanDays = new List<bool> { false, false, false, false, false };
            NumberOfStudents = 0;
            StartDate = DateTime.Now;
            Modifiable = true;
        }

        public Course ToCourse()
        {
            string[] timeParts = time.Split(':');
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            List<DayOfWeek> days = new();
            for (int i = 0; i < 5; i++)
            {
                if (booleanDays[i]) { days.Add((DayOfWeek)(i + 1)); }
            }
            return new Course(Id, TutorId, language, level, numberOfWeeks, days, online, NumberOfStudents, maxStudents, new DateTime(startDate.Year, startDate.Month, startDate.Day, hour, minute, 0), CreatedByDirector, Modifiable, GratitudeEmailSent);
        }

        public CourseViewModel(Course course)
        {
            Id = course.Id;
            Language = course.Language;
            Level = course.Level;
            NotOnline = !course.Online;
            CreatedByDirector = course.CreatedByDirector;
            TutorId = course.TutorId;
            NumberOfStudents = course.NumberOfStudents;
            StartDate = course.StartDateTime;
            Time = course.StartDateTime.ToString("HH:mm");

            NumberOfWeeks = course.NumberOfWeeks.ToString();
            MaxStudents = course.MaxStudents.ToString();
            Modifiable = course.Modifiable;
            SetDaysProperties(course.Days);
            DaysUntilEnd = course.DaysUntilEnd().ToString() + " until the end of course.";

            var tutorService = new TutorService();
            var tutor = tutorService.Get(course.TutorId);
            TutorFullName = tutor.Profile.Name + " " + tutor.Profile.LastName;
            
            var gradeService = new GradeService();
            GradedStudentsCount = gradeService.CountGradedStudents(course);
            GratitudeEmailSent = course.GratitudeEmailSent;
        }

        private void SetDaysProperties(List<DayOfWeek> days)
        {
            BooleanDays = new List<bool> { false, false, false, false, false };
            StringDays = string.Join(", ", days);
            for (int i = 0; i < 5; i++)
            {
                if (days.Contains((DayOfWeek)(i + 1))) { booleanDays[i] = true; }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
