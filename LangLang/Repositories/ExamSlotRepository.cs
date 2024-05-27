using LangLang.Configuration;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangLang.Repositories
{
    public class ExamSlotRepository : IExamSlotRepository
    {
        private readonly Dictionary<int, ExamSlot> _exams;
        private const string _filePath = Constants.FILENAME_PREFIX + "examSlots.csv";

        public ExamSlotRepository()
        {
            _exams = Load();
        }

        public ExamSlot? Get(int id)
        {
            return _exams[id];
        }

        public List<ExamSlot> GetAll()
        {
            return _exams.Values.ToList();
        }
        // Method to get all exam slots by tutor ID
        //function takes tutor id
        public List<ExamSlot> GetExams(Tutor tutor)
        {
            List<ExamSlot> exams = new List<ExamSlot>();

            foreach (ExamSlot exam in _exams.Values)
            {

                if (tutor.Id == exam.TutorId)
                {
                    exams.Add(exam);
                }
            }

            return exams;
        }

        //function takes examslot and adds it to dictionary of examslots
        //function saves changes and returns if adding was successful
        public void Add(ExamSlot exam)
        {
            _exams.Add(exam.Id, exam);
            Save();

        }
        //function for updating examslot takes new version of examslot and updates existing examslot to be same as new one
        //function saves changes and returns if updating was successful
        public void Update(ExamSlot exam)
        {
            ExamSlot? oldExam = Get(exam.Id);
            if (oldExam == null) return;

            oldExam.TutorId = exam.TutorId;
            oldExam.MaxStudents = exam.MaxStudents;
            oldExam.TimeSlot = exam.TimeSlot;
            oldExam.Modifiable = exam.Modifiable;
            oldExam.ResultsGenerated = exam.ResultsGenerated;
            oldExam.Applicants = exam.Applicants;
            oldExam.ExamineesNotified = exam.ExamineesNotified;

            Save();
        }
        //function takes id of examslot and removes examslot with that id
        //function saves changes and returns if removing was successful
        public void Delete(int id)
        {
            
            _exams.Remove(id);
            Save();

        }
        public void Save()
        {
            var lines = GetAll().Select(exam =>
            {
                return string.Join(Constants.DELIMITER,
                    exam.Id.ToString(),
                    exam.Language,
                    exam.Level.ToString(),
                    exam.TutorId.ToString(),
                    exam.TimeSlot.ToString(),
                    exam.MaxStudents.ToString(),
                    exam.Applicants.ToString(),
                    exam.Modifiable.ToString(),
                    exam.ResultsGenerated.ToString(),
                    exam.ExamineesNotified.ToString());
        });

            File.WriteAllLines(_filePath, lines);
        }

        public Dictionary<int, ExamSlot> Load()
        {
            var exams = new Dictionary<int, ExamSlot>();

            if (!File.Exists(_filePath)) return exams;

            var lines = File.ReadAllLines(_filePath);

            foreach (var line in lines)
            {
                string[] values = line.Split(Constants.DELIMITER);

                int id = int.Parse(values[0]);
                string language = values[1];
                LanguageLevel level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), values[2]);
                int tutorId = int.Parse(values[3]);
                TimeSlot timeSlot = new(values[4], values[5]);
                int maxStudents = int.Parse(values[6]);
                int applicants = int.Parse(values[7]);
                bool modifiable = bool.Parse(values[8]);
                bool resultsGenerated = bool.Parse(values[9]);
                bool examineesNotified = bool.Parse(values[10]);
                
                ExamSlot exam = new ExamSlot(id, language, level, timeSlot, maxStudents, tutorId, applicants, modifiable, resultsGenerated, examineesNotified);
                exams.Add(id, exam);

            }

            return exams;
            
        }
        /*
        private List<ExamSlot> Search(List<ExamSlot> exams, DateTime examDate, string language, LanguageLevel? level)
        {
            List<ExamSlot> filteredExams = exams.Where(exam =>
                (examDate == default || exam.TimeSlot.Time.Date == examDate.Date) &&
                (language == "" || exam.Language == language) &&
                (level == null || exam.Level == level)
            ).ToList();

            return filteredExams;
        }
        // Method to search exam slots by tutor and criteria
        public List<ExamSlot> SearchByTutor(Tutor tutor, DateTime examDate, string language, LanguageLevel? level)
        {
            List<ExamSlot> exams = _exams.Values.ToList();

            exams = GetExams(tutor);

            return Search(exams, examDate, language, level);
        }
        public List<ExamSlot> SearchByStudent(AppController appController, Student student, DateTime examDate, string language, LanguageLevel? level)
        {
            List<ExamSlot> availableExamSlots = GetAvailableExams(student, appController);
            return Search(availableExamSlots, examDate, language, level);
        }*/
    }
}