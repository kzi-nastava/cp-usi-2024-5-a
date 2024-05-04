using LangLang.Core.Model.Enums;
using LangLang.Core.Repository.Serialization;
using System;
namespace LangLang.Core.Model
{
    public class ExamAppRequest : ISerializable
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamSlotId { get; set; }
        public DateTime SentAt { get; set; }
        
        public ExamAppRequest() { }

        public ExamAppRequest(int id, int studentId, int examSlotId, DateTime sentAt)
        {
            Id = id;
            StudentId = studentId;
            ExamSlotId = examSlotId;
            SentAt = sentAt;
        }

        public void FromCSV(string[] values)
        {
            try
            {
                SentAt = DateTime.ParseExact(values[3], "yyyy-MM-dd", null);
            }
            catch
            {
                throw new FormatException("Date is not in the correct format.");
            }

            Id = int.Parse(values[0]);
            StudentId = int.Parse(values[1]);
            ExamSlotId = int.Parse(values[2]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                StudentId.ToString(),
                ExamSlotId.ToString(),
                SentAt.ToString("yyyy-MM-dd")
            };
        }
    }
}
