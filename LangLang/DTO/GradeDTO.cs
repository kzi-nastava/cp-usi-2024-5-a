﻿using LangLang.Core;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using System.ComponentModel;

namespace LangLang.DTO
{
    public class GradeDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        private int activityGrade;
        private int knowledgeGrade;
        public Course Course { get; set; }
        public Student Student {  get; set; }

        public int ActivityGrade
        {
            get => activityGrade;
            set
            {
                if (value != activityGrade)
                {
                    activityGrade = value;
                    OnPropertyChanged("ActivityGrade");
                }
            }
        }

        public int KnowledgeGrade
        {
            get => knowledgeGrade;
            set
            {
                if (value != knowledgeGrade)
                {
                    knowledgeGrade = value;
                    OnPropertyChanged("KnowledgeGrade");
                }
            }
        }

        public GradeDTO() { }

        public Grade ToGrade()
        {
            return new Grade(Id, CourseId, StudentId, ActivityGrade, KnowledgeGrade);
        }

        public GradeDTO(Grade grade, AppController appController)
        {
            Id = grade.Id;
            CourseId = grade.CourseId;
            StudentId = grade.StudentId;
            ActivityGrade = grade.ActivityGrade;
            KnowledgeGrade = grade.KnowledgeGrade;
            Course = appController.CourseController.Get(CourseId);
            Student = appController.StudentController.Get(StudentId);
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "ActivityGrade")
                {
                    if (activityGrade < Constants.MIN_GRADE || activityGrade > Constants.MAX_GRADE) 
                        return $"The rating must be between {Constants.MIN_GRADE} and {Constants.MAX_GRADE}";
                    
                    else return "";
                }

                if (columnName == "KnowledgeGrade")
                {
                    if (knowledgeGrade < Constants.MAX_GRADE || knowledgeGrade > Constants.MAX_GRADE) 
                        return $"The rating must be between {Constants.MIN_GRADE} and {Constants.MAX_GRADE}";
                    
                    else return "";
                }

                return "";
            }
        }

        private readonly string[] _validatedProperties = { "ActivityGrade", "KnowledgeGrade" };

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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
