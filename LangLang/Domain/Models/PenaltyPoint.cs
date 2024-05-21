using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Domain.Models
{
    public class PenaltyPoint : ISerializable
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TutorId { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }

        public PenaltyPoint() { }
        public PenaltyPoint(int id, int studentId, int tutorId, int courseId, DateTime date)
        {
            Id = id;
            StudentId = studentId;
            TutorId = tutorId;
            CourseId = courseId;
            Date = date;
        }

        public string[] ToCSV()
        {
            return new string[] {
            Id.ToString(),
            StudentId.ToString(),
            TutorId.ToString(),
            CourseId.ToString(),
            Date.ToString()
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            StudentId = int.Parse(values[1]);
            TutorId = int.Parse(values[2]);
            CourseId = int.Parse(values[3]);
            Date = DateTime.Parse(values[4]);
        }
    }
}
