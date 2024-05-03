﻿using LangLang.Core.Model;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace LangLang.DTO
{
    public class ExamSlotDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        
        public int Id { get; set; }

        private int _tutorId;
        private string? _language;
        private LanguageLevel _level;
        private int _maxStudents;
        private DateTime _examDate;

        private string? _time;
        //private int _numberStudents;
        private int _applicants;
        private bool _modifiable;

        public int TutorId
        {
            get { return _tutorId; }
            set
            {
                _tutorId = value;
                //OnPropertyChanged("TutorId");
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
                    OnPropertyChanged("MaxStudents");
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


        public int Applicants
        {
            get { return _applicants; }
            set
            {
                _applicants = value;
                OnPropertyChanged("Applicants");
            }
        }
        */
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }


        public bool Modifiable
        {
            get { return _modifiable; }
            set
            {
                _modifiable = value;
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

        public ExamSlotDTO()
        {
        }

        public ExamSlotDTO(ExamSlot examSlot)
        {
            this.Id = examSlot.Id;
            this.TutorId = examSlot.TutorId;
            this.Language = examSlot.Language;
            this.Level = examSlot.Level;
            this.MaxStudents = examSlot.MaxStudents.ToString();
            this.ExamDate = examSlot.TimeSlot.Time;
            this.Time = examSlot.TimeSlot.Time.ToString("HH:mm");
            this.Modifiable = examSlot.Modifiable;
            this.Applicants = examSlot.Applicants;
        }
        public string Error => null;

        public ExamSlot ToExamSlot()
        {
            Trace.Write(_time);
            string[] timeParts = _time.Split(':');
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            return new ExamSlot(Id, _language,_level, new TimeSlot(4,new DateTime(_examDate.Year, _examDate.Month, _examDate.Day, hour, minute, 0)),_maxStudents,_tutorId, _modifiable, _applicants);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
