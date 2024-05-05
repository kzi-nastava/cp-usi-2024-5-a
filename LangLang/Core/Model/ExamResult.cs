
using LangLang.Core.Model.Enums;
using LangLang.Core.Repository.Serialization;
using System;

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
        public ExamOutcome Outcome { get; set; }
        public ResultStatus Status { get; set; }

        public ExamResult(int id, int studentId, int examSlotId, int readingPoints, int speakingPoints, int listeningPoints, int writingPoints, ExamOutcome outcome, ResultStatus status)
        {
            Id = id;
            StudentId = studentId;
            ExamSlotId = examSlotId;
            ReadingPoints = readingPoints;
            SpeakingPoints = speakingPoints;
            ListeningPoints = listeningPoints;
            WritingPoints = writingPoints;
            Outcome = outcome;
            Status = status;
        }

        public ExamResult() {
            ListeningPoints = 0;
            SpeakingPoints = 0;
            WritingPoints = 0;
            ReadingPoints = 0;
            Outcome = ExamOutcome.NotGraded;
            Status = ResultStatus.Preliminary;
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
            Outcome = (ExamOutcome)Enum.Parse(typeof(ExamOutcome), values[7]);
            Status = (ResultStatus)Enum.Parse(typeof(ResultStatus), values[8]);
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
                WritingPoints.ToString(),
                Outcome.ToString(),
                Status.ToString()
            };
        }

        public void EvaluateOutcome()
        {
            bool readingPassed = ReadingPoints >= Constants.MIN_READING_POINTS;
            bool speakingPassed = SpeakingPoints >= Constants.MIN_SPEAKING_POINTS;
            bool listeningPassed = ListeningPoints >= Constants.MIN_LISTENING_POINTS;
            bool writingPassed = WritingPoints >= Constants.MIN_WRITING_POINTS;
            bool minimumAchieved = (ReadingPoints + SpeakingPoints + ListeningPoints + WritingPoints) >= Constants.MIN_TEST_POINTS;

            if (readingPassed && speakingPassed && writingPassed && listeningPassed && minimumAchieved) Outcome = ExamOutcome.Passed;
            Outcome =  ExamOutcome.Failed;
        }
    }
}
