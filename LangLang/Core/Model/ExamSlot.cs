using LangLang.Core.Repository.Serialization;
using System;

namespace LangLang.Core.Model
{
    public class ExamSlot: ISerializable
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public LanguageLevel Level { get; set; }
        
        // TODO: add tutor

        public TimeSlot TimeSlot { get; set; }
        public int MaxStudents { get; set; }
        public int StudentsNumber { get; set; } // TODO: mozda ne treba broj prijavljenih nego mozes da pitas abzu

        public ExamSlot(int id, string language, LanguageLevel level, TimeSlot timeSlot, int maxStudents, DateTime examDateTime, int studentsNumber)
        {
            Id = id;
            Language = language;
            Level = Level;
            TimeSlot = timeSlot;
            MaxStudents = maxStudents;
            StudentsNumber = studentsNumber;
        }

        public ExamSlot() { }

        public string[] ToCSV()
        {
            return new string[]
            {
            Id.ToString(),
            Language,
            Level.ToString(),
            MaxStudents.ToString(),
            TimeSlot.ToString(),
            StudentsNumber.ToString()
            };
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Language = values[1];
            Level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), values[2]);
            MaxStudents = int.Parse(values[3]);
            // TODO: add exam slot
            StudentsNumber = int.Parse(values[5]);
        }
    }
}
