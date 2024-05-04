using LangLang.Core.Repository.Serialization;
using System;

namespace LangLang.Core.Model
{
    public class ExamSlot: ISerializable
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public LanguageLevel Level { get; set; }
        public int TutorId { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int MaxStudents { get; set; }
        public bool Modifiable { get; set; }

        // NOTE: if possible don't save number of registeredStudents, ask the database. If not, then add attribute.

        public bool GeneratedResults { get; set; }
        public ExamSlot(int id, string language, LanguageLevel level, TimeSlot timeSlot, int maxStudents, int tutorId, bool modifiable, bool generatedResults)
        {
            Id = id;
            Language = language;
            Level = Level;
            TutorId = tutorId;
            TimeSlot = timeSlot;
            MaxStudents = maxStudents;
            Modifiable = modifiable;
            GeneratedResults = generatedResults;
        }

        public ExamSlot() { }

        public string[] ToCSV()
        {
            return new string[] {
            Id.ToString(),
            Language,
            Level.ToString(),
            TutorId.ToString(),
            TimeSlot.ToString(),
            MaxStudents.ToString(),
            Modifiable.ToString(),
            GeneratedResults.ToString()
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
            Modifiable = bool.Parse(values[7]);
            GeneratedResults = bool.Parse(values[8]);
        }

    }
}
