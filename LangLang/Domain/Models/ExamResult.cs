
using LangLang.Core;
using LangLang.Core.Model.Enums;

namespace LangLang.Domain.Models
{
    public class ExamResult
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

        public ExamResult(int studentId, int examId)
        {
            StudentId = studentId;
            ExamSlotId = examId;
        }
        public ExamResult() {}

        public void EvaluateOutcome()
        {
            bool readingPassed = ReadingPoints >= Constants.MIN_READING_POINTS;
            bool speakingPassed = SpeakingPoints >= Constants.MIN_SPEAKING_POINTS;
            bool listeningPassed = ListeningPoints >= Constants.MIN_LISTENING_POINTS;
            bool writingPassed = WritingPoints >= Constants.MIN_WRITING_POINTS;
            bool minimumAchieved = (ReadingPoints + SpeakingPoints + ListeningPoints + WritingPoints) >= Constants.MIN_TEST_POINTS;

            if (readingPassed && speakingPassed && writingPassed && listeningPassed && minimumAchieved) Outcome = ExamOutcome.Passed;
            else Outcome =  ExamOutcome.Failed;
        }
    }
}
