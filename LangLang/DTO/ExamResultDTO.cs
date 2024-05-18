
using LangLang.Aplication.UseCases;
using LangLang.Core;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Core.Model.Enums;
using LangLang.Domain.Models;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace LangLang.DTO
{
    public class ExamResultDTO: INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ResultStatus Status { get; set; }

        private string readingPoints;
        private string speakingPoints;
        private string writingPoints;
        private string listeningPoints;
        private ExamOutcome outcome;


        public string ReadingPoints
        {
            get { return readingPoints; }
            set
            {
                if (value != readingPoints)
                {
                    readingPoints = value;
                    OnPropertyChanged("ReadingPoints");
                }
            }
        }

        public string SpeakingPoints
        {
            get { return speakingPoints; }
            set
            {
                if (value != speakingPoints)
                {
                    speakingPoints = value;
                    OnPropertyChanged("SpeakingPoints");
                }
            }
        }

        public string ListeningPoints
        {
            get { return listeningPoints; }
            set
            {
                if (value != listeningPoints)
                {
                    listeningPoints = value;
                    OnPropertyChanged("ListeningPoints");
                }
            }
        }

        public string WritingPoints
        {
            get { return writingPoints; }
            set
            {
                if (value != writingPoints)
                {
                    writingPoints = value;
                    OnPropertyChanged("WritingPoints");
                }
            }
        }

        public ExamOutcome Outcome
        {
            get { return outcome; }
            set
            {
                if (value != outcome)
                {
                    outcome = value;
                    OnPropertyChanged("Outcome");
                }
            }
        }

        private readonly Regex _numberRegex = new("^\\+?\\d+$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "ListeningPoints")
                {
                    if (string.IsNullOrEmpty(ListeningPoints)) return "Listening points are required";
                    if (!_numberRegex.Match(ListeningPoints).Success) return "The field must consist of numbers.";
                    if (int.Parse(listeningPoints) > Constants.MAX_LISTENING_POINTS)  return $"The maximum number of points is {Constants.MAX_LISTENING_POINTS}.";
                    else return "";
                }

                if (columnName == "ReadingPoints")
                {
                    if (string.IsNullOrEmpty(ReadingPoints)) return "Reading points are required";
                    if (!_numberRegex.Match(ReadingPoints).Success) return "The field must consist of numbers.";
                    if (int.Parse(ReadingPoints) > Constants.MAX_READING_POINTS) return $"The maximum number of points is {Constants.MAX_READING_POINTS}.";
                    else return "";
                }

                if (columnName == "SpeakingPoints")
                {
                    if (string.IsNullOrEmpty(SpeakingPoints)) return "Speaking points are required";
                    if (!_numberRegex.Match(SpeakingPoints).Success) return "The field must consist of numbers.";
                    if (int.Parse(SpeakingPoints) > Constants.MAX_SPEAKING_POINTS) return $"The maximum number of points is {Constants.MAX_SPEAKING_POINTS}";
                    else return "";
                }

                if (columnName == "WritingPoints")
                {
                    if (string.IsNullOrEmpty(WritingPoints)) return "Writing points are required.";
                    if (!_numberRegex.Match(WritingPoints).Success) return "The field must consist of numbers.";
                    if (int.Parse(WritingPoints) > Constants.MAX_WRITING_POINTS) return $"The maximum number of points is {Constants.MAX_WRITING_POINTS}.";
                    else return "";
                }
                return "";
            }
        }

        private readonly string[] _validatedProperties = { "ListeningPoints", "ReadingPoints", "WritingPoints", "SpeakingPoints"};

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

        public ExamResultDTO() { }

        public ExamResult ToExamResult()
        {
            return new ExamResult(Id, StudentId, ExamId, int.Parse(readingPoints), int.Parse(speakingPoints), int.Parse(listeningPoints), int.Parse(writingPoints), outcome, Status);
        }

        public ExamResultDTO(ExamResult examResult)
        {
            var studentService = new StudentService();
            Student student = studentService.Get(examResult.StudentId);

            Id = examResult.Id;
            StudentId = examResult.StudentId;
            Name = student.Profile.Name;
            LastName = student.Profile.LastName;
            Email = student.Profile.Email;
            ReadingPoints = examResult.ReadingPoints.ToString();
            SpeakingPoints = examResult.SpeakingPoints.ToString();
            ListeningPoints = examResult.ListeningPoints.ToString();
            WritingPoints = examResult.WritingPoints.ToString();
            Outcome = examResult.Outcome;
            Status = examResult.Status;
        }


        public string Error => throw new NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
