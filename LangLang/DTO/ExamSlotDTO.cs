using LangLang.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LangLang.DTO
{
    public class ExamSlotDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        public ExamSlotDTO() { }
        public int Id { get; set; }

        private int _courseId;
        private string _language;
        private LanguageLevel _level;
        private int _maxStudents;
        private DateTime _examDate;
        private string _time;
        private int _numberOfStudents;
        private DateTime _examDateTime;

        public int CourseId
        {
            get { return _courseId; }
            set
            {
                _courseId = value;
                OnPropertyChanged("CourseId");
            }
        }

        public string Language
        {
            get { return _language; }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        public LanguageLevel Level
        {
            get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    OnPropertyChanged("Level");
                }
            }
        }

        public string MaxStudents
        {
            get { return _maxStudents.ToString(); }
            set
            {

                if (int.TryParse(value, out int result) && result >= 0)
                {
                    _maxStudents = result;
                    //OnPropertyChanged("MaxStudents");
                }
                else
                {
                    _maxStudents = 0;
                }

            }
        }

        public DateTime ExamDate
        {
            get { return _examDate; }
            set
            {
                _examDate = value;
                OnPropertyChanged("ExamDate");
            }
        }

        public int NumberOfStudents
        {
            get { return _numberOfStudents; }
            set
            {
                _numberOfStudents = value;
                //OnPropertyChanged("NumberOfStudents");
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        public DateTime ExamDateTime
        {
            get { return _examDateTime; }
            set 
            {
                _examDateTime = value;
            }
        }

        private readonly Regex _TimeRegex = new("^(?:[01]\\d|2[0-3]):(?:[0-5]\\d)$");


        public string this[string columnName]
        {
            get
            {

                if (columnName == "MaxStudents")
                {

                    if (string.IsNullOrEmpty(MaxStudents)) return "Max number of students is required";
                    Trace.WriteLine("MMMMMM "+MaxStudents);
                    if (!int.TryParse(MaxStudents, out int _maxStudents) || _maxStudents <= 0) return "Must input a positive integer for max number of students.";
                    else return "";
                }

                if (columnName == "ExamDate")
                {
                    if (_examDate <= DateTime.Now) return "Please enter a future date.";
                    if (_examDate == default) return "Exam date is required";
                    else return "";
                }

                if (columnName == "Time")
                {
                    if (string.IsNullOrEmpty(Time)) return "Time is required";
                    if (!_TimeRegex.Match(Time).Success) return "Time must be of format hh:mm .";
                    else return "";
                }
                return "";
            }
        }

        private readonly string[] _validatedProperties = { "MaxStudents", "ExamDate", "Time" };

        // checks if all properties are valid
        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    Trace.WriteLine(this[property]);
                    if (this[property] != "")
                        return false;
                }

                return true;
            }
        }



        public ExamSlotDTO(ExamSlot examSlot, Course course)
        {
            this.Id = examSlot.Id;
            this.Language = course.Language;
            this.Level = course.Level;
            this.MaxStudents = examSlot.MaxStudents.ToString();
            this.NumberOfStudents = examSlot.NumberOfStudents;
            this.CourseId = examSlot.CourseId;
            this.ExamDate = examSlot.ExamDateTime.Date;
            this.Time = examSlot.ExamDateTime.ToString("HH:mm");
            this.ExamDateTime = examSlot.ExamDateTime;
        }

        private DateTime ToDateTime(DateTime date, string time)
        {

            string[] timeComponents = time.Split(':');

            if (timeComponents.Length == 2)
            {
                // Parse hour component
                if (int.TryParse(timeComponents[0], out int hours))
                {
                    // Parse minute component
                    if (int.TryParse(timeComponents[1], out int minutes))
                    {
                        // Create a new DateTime object with the combined date and time components
                        DateTime combinedDateTime = new DateTime(date.Year, date.Month, date.Day, hours, minutes, 0);

                    }
                    else
                    {
                        Console.WriteLine("Invalid minute component.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid hour component.");
                }
            }
            else
            {
                Console.WriteLine("Invalid time format.");
            }
            return DateTime.Now;
        }

        public ExamSlot ToExamSlot()
        {
            string[] timeParts = _time.Split(':');
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            return new ExamSlot(Id, _courseId, _maxStudents, new DateTime(_examDate.Year, _examDate.Month, _examDate.Day, hour, minute, 0), 0);
        }

        public string Error => throw new NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
