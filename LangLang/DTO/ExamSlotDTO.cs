using LangLang.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            }
        }

        public string MaxStudents
        {
            get { return _maxStudents.ToString(); ; }
            set
            {
                if (int.TryParse(value, out int result) && result >= 0)
                {
                    _maxStudents = result;
                }
                else
                {
                    throw new ArgumentException("Max number of students must be a non-negative integer.");
                }
            }
        }

        public DateTime ExamDate
        {
            get { return _examDate; }
            set
            {
                _examDate = value;
            }
        }

        public int NumberOfStudents
        {
            get { return _numberOfStudents; }
            set
            {
                _numberOfStudents = value;
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
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
                    if (!int.TryParse(MaxStudents, out int maxStudents) || maxStudents <= 0) return "Must input a positive integer for max number of students.";
                    else return "";
                }

                if (columnName == "ExamDate")
                {
                    if (_examDate <= DateTime.Now) return "Please enter a future date.";
                    if (_examDate == default) return "Exam date is required";
                    else return "";
                }

                if (columnName == "ExamTime")
                {
                    if (string.IsNullOrEmpty(Time)) return "Time is required";
                    if (!_TimeRegex.Match(Time).Success) return "Time must be of format hh:mm .";
                    else return "";
                }
                return "";
            }
        }

        private readonly string[] _validatedProperties = { "MaxStudents", "ExamDate", "ExamTime" };

        // checks if all properties are valid
        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return true;
                }

                return true;
            }
        }



        public ExamSlotDTO(ExamSlot examSlot)
        {
            this.Id = examSlot.Id;
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
            return new ExamSlot(Id, _courseId, _maxStudents, ToDateTime(_examDate,_time) , 0);
        }

        public string Error => throw new NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
