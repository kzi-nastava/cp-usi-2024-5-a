using LangLang.Configuration;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.Interfaces;
using LangLang.WPF.ViewModels.Validations;
using System;
using System.Collections.Generic;
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

        //validation rules
        private readonly List<IValidationRule<string>> maxStudentsRules;
        private readonly List<IValidationRule<DateTime>> dateRules;
        private readonly List<IValidationRule<string>> timeRules;

        public ExamSlotViewModel()
        {
            maxStudentsRules = new List<IValidationRule<string>>
            {
                new RequiredFieldRule<string>("Max number of students is required."),
                new PositiveNumberRule()
            };
            dateRules = new List<IValidationRule<DateTime>> {
                    new RequiredFieldRule<DateTime>(),
                    new FutureDateRule() 
            };
            timeRules = new List<IValidationRule<string>> { 
                new RequiredFieldRule<string>("Time is required."),
                new TimeFormatRule() 
            };
        }

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


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(MaxStudents):
                        error = ValidateProperty(MaxStudents, maxStudentsRules);
                        break;
                    case nameof(ExamDate):
                        error = ValidateProperty(ExamDate, dateRules);
                        break;
                    case nameof(Time):
                        error = ValidateProperty(Time, timeRules);
                        break;
                }
                return error;
            }
        }

        private string ValidateProperty<T>(T value, List<IValidationRule<T>> rules)
        {
            foreach (var rule in rules)
            {
                if (!rule.Validate(value))
                {
                    return rule.ErrorMessage;
                }
            }
            return string.Empty;
        }

        private readonly string[] _validatedProperties = { nameof(MaxStudents), nameof(ExamDate), nameof(Time) };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != string.Empty)
                        return false;
                }
                return true;
            }
        }

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
