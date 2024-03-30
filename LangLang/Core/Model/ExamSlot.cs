using LangLang.Core.Repository.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Model
{
    public class ExamSlot: ISerializable
    {
        // Private attributes
        private int _id;
        private int _courseId;
        private int _maxStudents;
        private DateTime _examDateTime;
        private int _numberOfStudents;

        // Public properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Course
        {
            get { return _courseId; }
            set { _courseId = value; }
        }

        public int MaxStudents
        {
            get { return _maxStudents; }
            set { _maxStudents = value; }
        }

        public DateTime ExamDateTime
        {
            get { return _examDateTime; }
            set { _examDateTime = value; }
        }

        public int NumberOfStudents
        {
            get { return _numberOfStudents; }
            set { _numberOfStudents = value; }
        }

        // Constructor
        public ExamSlot(int id, int courseId, int maxStudents, DateTime examDateTime, int numberOfStudents)
        {
            _id = id;
            _courseId = courseId;
            _maxStudents = maxStudents;
            _examDateTime = examDateTime;
            _numberOfStudents = numberOfStudents;
        }

        public ExamSlot(int id, int courseId, int maxStudents, DateTime examDateTime)
        {
            _id = id;
            _courseId = courseId;
            _maxStudents = maxStudents;
            _examDateTime = examDateTime;
            _numberOfStudents = 0;
        }

        public ExamSlot() { }

        // Methods

        // Method to display information about the exam slot
        public override string ToString()
        {
            return $"ID: {_id} | Course: {_courseId.ToString()} | Max Students: {_maxStudents} | Exam Date and Time: {_examDateTime} | Number of Students: {_numberOfStudents}";
        }

        // Method to convert object to CSV format
        //returns array of strings representing attributes of ExamSlot object in string form
        public string[] ToCSV()
        {
            string[] csvValues =
            {
            _id.ToString(),
            _courseId.ToString(),
            _maxStudents.ToString(),
            _examDateTime.ToString(),
            _numberOfStudents.ToString()
        };
            return csvValues;
        }
        // Method to make object from CSV data
        //takes array of strings representing ExamSlot object values and sets attributes of object to that values
        public void FromCSV(string[] values)
        {
            _id = int.Parse(values[0]);
            _courseId = int.Parse(values[1]);
            _maxStudents = int.Parse(values[2]);
            _examDateTime = DateTime.Parse(values[3]);
            _numberOfStudents = int.Parse(values[4]);
        }
    }
}
