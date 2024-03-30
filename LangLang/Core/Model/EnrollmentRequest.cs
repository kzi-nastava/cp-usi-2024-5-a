﻿using LangLang.Core.Model.Enums;
using LangLang.Core.Repository.Serialization;
using System;

namespace LangLang.Core.Model
{
    public class EnrollmentRequest : ISerializable
    {
        private int _id;
        private int _studentId;
        private int _courseId;
        private ERStatus _erStatus;
        private DateTime _requestSentAt;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int StudentId
        {
            get { return _studentId; }
            set { _studentId = value; }
        }

        public int CourseId
        {
            get { return _courseId; }
            set { _courseId = value; }
        }

        public DateTime RequestSentAt
        {
            get { return _requestSentAt; }
            set { _requestSentAt = value; }
        }

        public ERStatus ERStatus
        {
            get { return _erStatus; }
            set { _erStatus = value; }
        }

        public EnrollmentRequest() {}

        public EnrollmentRequest(int id, int studentId, int courseId, ERStatus erStatus, DateTime requestSentAt)
        {
            Id = id;
            StudentId = studentId;
            CourseId = courseId;
            ERStatus = erStatus;
            RequestSentAt = requestSentAt;
        }

        public void FromCSV(string[] values)
        {
            if (!int.TryParse(values[0], out _id)
                || !int.TryParse(values[1], out _studentId)
                || !int.TryParse(values[2], out _courseId)
                || !Enum.TryParse(values[3], out _erStatus)
                || !DateTime.TryParseExact(values[4], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _requestSentAt))
                {
                    throw new FormatException("Error during parsing while reading from file");  
                }
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                StudentId.ToString(),
                CourseId.ToString(),
                ERStatus.ToString(),
                RequestSentAt.ToString("yyyy-MM-dd")
            };
            return values;
        }
    }
}
