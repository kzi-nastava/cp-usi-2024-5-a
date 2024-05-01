
using LangLang.Core.Repository.Serialization;

namespace LangLang.Core.Model
{
    public class ExamResult : ISerializable
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamSlotId { get; set; }
        public int ReadingPoints { get; set; }
        public int SpeakingPoints { get; set; }
        public int WritingPoints { get; set; }
        public int ListeningPoints { get; set; }

        public ExamResult(int id, int studentId, int examSlotId, int readingPoints, int speakingPoints, int listeningPoints, int writingPoints)
        {
            Id = id;
            StudentId = studentId;
            ExamSlotId = examSlotId;
            ReadingPoints = readingPoints;
            SpeakingPoints = speakingPoints;
            ListeningPoints = listeningPoints;
            WritingPoints = writingPoints;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            StudentId = int.Parse(values[1]);
            ExamSlotId = int.Parse(values[2]);
            ReadingPoints = int.Parse(values[3]);
            SpeakingPoints = int.Parse(values[4]);
            ListeningPoints = int.Parse(values[5]);
            WritingPoints = int.Parse(values[6]);
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                StudentId.ToString(),
                ExamSlotId.ToString(),
                ReadingPoints.ToString(),
                SpeakingPoints.ToString(),
                ListeningPoints.ToString(),
                WritingPoints.ToString()
            };
        }
    }
}
