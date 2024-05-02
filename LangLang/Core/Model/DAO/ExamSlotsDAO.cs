using LangLang.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Core.Observer;
using System.Collections;
using System.Windows.Input;
using LangLang.Core.Controller;
using LangLang.View.ExamSlotGUI;

namespace LangLang.Core.Model.DAO
{
    public class ExamSlotsDAO : Subject
    {
        private readonly Dictionary<int, ExamSlot> _exams;
        private readonly Repository<ExamSlot> _repository;

        public ExamSlotsDAO()
        {
            _repository = new Repository<ExamSlot>("examSlots.csv");
            _exams = _repository.Load();
        }
        private int GenerateId()
        {
            if (_exams.Count == 0) return 0;
            return _exams.Keys.Max() + 1;
        }

        public ExamSlot? GetExamById(int id)
        {
            return _exams[id];
        }

        public Dictionary<int, ExamSlot> GetAllExams()
        {
            return _exams;
        }


        //function takes examslot and adds it to dictionary of examslots
        //function saves changes and returns if adding was successful
        public bool AddExam(ExamSlot exam, CourseController courses)
        {
            return false;
            /*
            if (CanCreateExamSlot(exam, courses))
            {
                exam.Id = GenerateId();
                _exams[exam.Id] = exam;
                _repository.Save(_exams);
                NotifyObservers();
                return true;
            }
            else
            {
                return false;
            }
            */
        }

        //function takes id of examslot and removes examslot with that id
        //function saves changes and returns if removing was successful
        public bool RemoveExam(int id)
        {
            ExamSlot exam = GetExamById(id);
            if (exam == null) return false;

            //should use const variable instead of 14
            if ((exam.TimeSlot.Time - DateTime.Now).TotalDays >= 14)
            {
                _exams.Remove(id);
                _repository.Save(_exams);
                NotifyObservers();
                return true;
            }
            else
            {
                return false;
            }

        }

        //function for updating examslot takes new version of examslot and updates existing examslot to be same as new one
        //function saves changes and returns if updating was successful
        public bool UpdateExam(ExamSlot exam)
        {
            //should use const variable instead of 14

            if ((exam.TimeSlot.Time - DateTime.Now).TotalDays >= 14)
            {
                ExamSlot oldExam = GetExamById(exam.Id);
                if (oldExam == null) return false;

                oldExam.TutorId = exam.TutorId;
                oldExam.MaxStudents = exam.MaxStudents;
                oldExam.TimeSlot = exam.TimeSlot;
                oldExam.Modifiable = exam.Modifiable;

                _repository.Save(_exams);
                NotifyObservers();
                return true;
            }
            else
            {
                return false;
            }

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


        // Method to check if an exam slot is available
        // takes exam slot, returns true if it is availbale or false if it isn't available
        public bool IsAvailable(ExamSlot exam, ExamAppRequestController examAppController)
        {
            if (HasPassed(exam))
            {
                return false;
            }


            if (IsFullyBooked(exam, examAppController))
            {
                return false;
            }

            return true;
        }

        public bool HasPassed(ExamSlot exam)
        {
            return exam.TimeSlot.Time > DateTime.Now;
        }
        public int CountExamApplications(ExamSlot exam, ExamAppRequestController examAppController)
        {
            int count = 0;
            foreach (ExamAppRequest request in examAppController.GetAll())
            {
                if (request.ExamSlotId == exam.Id && !request.IsCanceled)
                {
                    count++;
                }
            }
            return count;
        }

        public bool IsFullyBooked(ExamSlot exam, ExamAppRequestController examAppController)
        {
            return exam.MaxStudents == CountExamApplications(exam, examAppController);
        }

        // Method to search exam slots by tutor and criteria
        public List<ExamSlot> SearchExamsByTutor(Tutor tutor, DateTime examDate, string language, LanguageLevel? level)
        {
            List<ExamSlot> exams = _exams.Values.ToList();

            exams = this.GetExams(tutor);

            return SearchExams(exams, examDate, language, level);
        }
        public List<ExamSlot> SearchExams(List<ExamSlot> exams, DateTime examDate, string language, LanguageLevel? level)
        {

            // Apply search criteria if they are not null
            List<ExamSlot> filteredExams = exams.Where(exam =>
                (examDate == default || exam.TimeSlot.Time.Date == examDate.Date) &&
                (language == "" || exam.Language == language) &&
                (level == null || exam.Level == level)
            ).ToList();

            return filteredExams;
        }


        public bool ApplicationsVisible(int id)
        {
            ExamSlot examSlot = GetExamById(id);
            return examSlot.ApplicationsVisible();
        }

    }
}
