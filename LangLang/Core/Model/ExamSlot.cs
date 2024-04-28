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

        // NOTE: if possible don't save number of registeredStudents, ask the database. If not, then add attribute.
        

        public ExamSlot(int id, string language, LanguageLevel level, TimeSlot timeSlot, int maxStudents, int tutorId)
        {
            Id = id;
            Language = language;
            Level = Level;
            TutorId = tutorId;
            TimeSlot = timeSlot;
            MaxStudents = maxStudents;
        }

        public ExamSlot() { }

        public string[] ToCSV()
        {
            return new string[] {
            Id.ToString(),
            Language,
            Level.ToString(),
            TutorId.ToString(),
            // TODO: add serialization for timeSlot when implemented
            MaxStudents.ToString(),
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Language = values[1];
            Level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), values[2]);
            TutorId = int.Parse(values[3]);
            // TODO: add deserialization for timeSlot when implemented
            MaxStudents = int.Parse(values[6]);
        }
    }
}
