using LangLang.Core.Repository.Serialization;
using System;

namespace LangLang.Core.Model
{
    public class ExamSlot: ISerializable, Overlapable
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public LanguageLevel Level { get; set; }
        public int TutorId { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int MaxStudents { get; set; }
        public bool ApplicationPossible { get; set; }

        // NOTE: if possible don't save number of registeredStudents, ask the database. If not, then add attribute.
        

        public ExamSlot(int id, string language, LanguageLevel level, TimeSlot timeSlot, int maxStudents, int tutorId, bool applicationPossible)
        {
            Id = id;
            Language = language;
            Level = Level;
            TutorId = tutorId;
            TimeSlot = timeSlot;
            MaxStudents = maxStudents;
            ApplicationPossible = applicationPossible;
        }

        public string[] ToCSV()
        {
            return new string[] {
            Id.ToString(),
            Language,
            Level.ToString(),
            TutorId.ToString(),
            TimeSlot.ToString(),
            MaxStudents.ToString(),
            ApplicationPossible.ToString()
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Language = values[1];
            Level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), values[2]);
            TutorId = int.Parse(values[3]);
            TimeSlot = new (values[4], values[5]);
            MaxStudents = int.Parse(values[6]);
            ApplicationPossible = bool.Parse(values[7]);
        }

    }
}
