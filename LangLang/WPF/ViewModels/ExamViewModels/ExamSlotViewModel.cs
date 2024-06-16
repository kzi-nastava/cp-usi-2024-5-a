using LangLang.Configuration;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;


namespace LangLang.WPF.ViewModels.ExamViewModels
{
    public class ExamSlotViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        public int Id { get; set; }

        public int TutorId { get; set; }
        private string language;
        private LanguageLevel level;
        private int maxStudents;
        private DateTime examDate;
        private string time;
        private int applicants;
        public bool Modifiable { get; set; }
        public bool ResultsGenerated { get; set; }
        public bool ExamineesNotified { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Language
        {
            get { return language; }
            set
            {
                if (language != value)
                {
                    language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        public LanguageLevel Level
        {
            get { return level; }
            set
            {
                if (level != value)
                {
                    level = value;
                    OnPropertyChanged("Level");
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
                    OnPropertyChanged("MaxStudents");
                }
                else
                {
                    maxStudents = 0;
                }

            }
        }

        public DateTime ExamDate
        {
            get { return examDate; }
            set
            {
                if (examDate != value)
                {
                    examDate = value;
                    OnPropertyChanged("ExamDate");
                }
            }
        }


        public int Applicants
        {
            get { return applicants; }
            set
            {
                if (applicants != value)
                {
                    applicants = value;
                    OnPropertyChanged("Applicants");
                }
            }
        }

        public string Time
        {
            get { return time; }
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged("Time");
                }
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
                    if (examDate <= DateTime.Now) return "Please enter a future date.";
                    if (examDate == default) return "Exam date is required";
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
                    if (this[property] != "")
                        return false;
                }

                return true;
            }
        }

        public ExamSlotViewModel() { }

        public ExamSlotViewModel(ExamSlot examSlot)
        {
            Id = examSlot.Id;
            TutorId = examSlot.TutorId;
            Language = examSlot.Language;
            Level = examSlot.Level;
            MaxStudents = examSlot.MaxStudents.ToString();
            ExamDate = examSlot.TimeSlot.Time;
            Time = examSlot.TimeSlot.Time.ToString("HH:mm");
            Applicants = examSlot.Applicants;
            Modifiable = examSlot.Modifiable;
            ResultsGenerated = examSlot.ResultsGenerated;
            ExamineesNotified = examSlot.ExamineesNotified;
        }
        public string Error => null;

        public ExamSlot ToExamSlot()
        {
            string[] timeParts = time.Split(':');
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            return new ExamSlot(Id, language, level, new TimeSlot(Constants.EXAM_DURATION, new DateTime(examDate.Year, examDate.Month, examDate.Day, hour, minute, 0)), maxStudents, TutorId, applicants, Modifiable, ResultsGenerated, ExamineesNotified, CreatedAt);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
