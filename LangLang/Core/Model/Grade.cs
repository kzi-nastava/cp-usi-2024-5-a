using LangLang.Core.Repository.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Model
{
    public class Grade : ISerializable
    {
        private int _id;
        private int _courseId;
        private int _studentId;
        private int _gradeValue;
        public int Id
        {
            get { return _id; }
            set { _id = value; } 
        }
        public int CourseId
        { 
          get { return _courseId; }
          set { _courseId = value; } 
        }
        public int StudentId
        { 
          get { return _studentId; }
          set { _studentId = value; } 
        }
        public int GradeValue
        {
            get => _gradeValue;
            set
            {
                if (value < Constants.MIN_GRADE || value > Constants.MAX_GRADE)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"Rating must be between {Constants.MIN_GRADE} and {Constants.MAX_GRADE}.");
                }
                _gradeValue = value;
            }
        }

        public Grade(int id, int courseId, int studentId, int gradeValue)
        {
            Id = id;
            CourseId = courseId;
            StudentId = studentId;
            GradeValue = gradeValue;
        }

        public Grade() { }

        public string[] ToCSV()
        {
            string[] csvValues = { _id.ToString(), _courseId.ToString(), _studentId.ToString(), _gradeValue.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            try
            {
                _id = int.Parse(values[0]);
                _courseId = int.Parse(values[1]);
                _studentId = int.Parse(values[2]);
                _gradeValue = int.Parse(values[3]);
            }
            catch
            {
                throw;
            }
        }
    }
}
