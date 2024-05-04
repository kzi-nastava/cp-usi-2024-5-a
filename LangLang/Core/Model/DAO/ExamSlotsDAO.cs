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
using System.Diagnostics;
using LangLang.Core.Model.Enums;

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
            
        }

        public bool CanCreateExamSlot(ExamSlot exam, CourseController courseController)
        {
            int busyClassrooms = 0;
            return !CoursesAndExamOverlapp(exam, courseController,ref busyClassrooms) && !ExamsOverlapp(exam,ref busyClassrooms);   
        }
        // Checks for any overlaps between courses and the exam, considering the availability of the exam's tutor and classrooms
        public bool CoursesAndExamOverlapp(ExamSlot exam, CourseController courseController, ref int busyClassrooms)
        {
            List<Course> courses = courseController.GetAllCourses().Values.ToList();
            // Go through courses
            foreach (Course course in courses)
            {
                //check for overlapping
                if (courseController.OverlappsWith(course, exam.TimeSlot))
                {
                    //tutor is busy (has class)
                    if (course.TutorId == exam.TutorId)
                    {
                        return true;
                    }

                    if (!course.Online)
                    {
                        busyClassrooms++;
                    }

                    //all classrooms are busy
                    if (busyClassrooms == 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Checks for any overlaps between other exams and current exam, considering the availability of the exam's tutor and classrooms
        public bool ExamsOverlapp(ExamSlot exam, ref int busyClassrooms)
        {
            // Go through all exams
            foreach (ExamSlot currExam in GetAllExams().Values)
            {
                if (exam.TimeSlot.OverlappsWith(currExam.TimeSlot))
                {
                    //tutor is busy (has exam)
                    if (exam.TutorId == currExam.TutorId)
                    {
                        return true;
                    }

                    busyClassrooms++;

                    //all classrooms are busy
                    if (busyClassrooms == 2)
                    {
                        return true;
                    }
                }

            }
            return false;
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
        public void AddStudent(ExamSlot exam)
        {
            exam.Applicants++;
            _repository.Save(_exams);
            NotifyObservers();
        }

        public void RemoveStudent(ExamSlot exam)
        {
            exam.Applicants--;
            _repository.Save(_exams);
            NotifyObservers();
        }
        //function for updating examslot takes new version of examslot and updates existing examslot to be same as new one
        //function saves changes and returns if updating was successful
        public void UpdateExam(ExamSlot exam)
        {
            ExamSlot? oldExam = GetExamById(exam.Id);
            if (oldExam == null) return;

            oldExam.TutorId = exam.TutorId;
            oldExam.MaxStudents = exam.MaxStudents;
            oldExam.TimeSlot = exam.TimeSlot;
            oldExam.Modifiable = exam.Modifiable;
            oldExam.GeneratedResults = exam.GeneratedResults;
            oldExam.Applicants = exam.Applicants;

            _repository.Save(_exams);
            NotifyObservers();
        }

        public bool CanBeUpdated(ExamSlot exam)
        {
            return (exam.TimeSlot.Time - DateTime.Now).TotalDays >= 14;
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
        public bool IsAvailable(ExamSlot exam)
        {

            if (HasPassed(exam))
            {
                return false;
            }


            if (IsFullyBooked(exam))
            {
                return false;
            }

            return true;
        }

        public bool HasPassed(ExamSlot exam)
        {
            return exam.TimeSlot.Time < DateTime.Now;
        }
        /*
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
        */
        public bool IsFullyBooked(ExamSlot exam)
        {
            return exam.MaxStudents == exam.Applicants;
        }

        // Method to search exam slots by tutor and criteria
        public List<ExamSlot> SearchExamsByTutor(Tutor tutor, DateTime examDate, string language, LanguageLevel? level)
        {
            List<ExamSlot> exams = _exams.Values.ToList();

            exams = this.GetExams(tutor);

            return SearchExams(exams, examDate, language, level);
        }

        private List<ExamSlot> SearchExams(List<ExamSlot> exams, DateTime examDate, string language, LanguageLevel? level)
        {
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

        public List<ExamSlot> SearchExamsByStudent(AppController appController, Student student, DateTime examDate, string courseLanguage, LanguageLevel? languageLevel)
        {
            List<ExamSlot> availableExamSlots = GetAvailableExams(student, appController);
            return SearchExams(availableExamSlots, examDate, courseLanguage, languageLevel);
        }


        // returns a list of exams that are available for student application
        public List<ExamSlot> GetAvailableExams(Student student, AppController appController)
        {
            var courseController = appController.CourseController;
            var enrollmentController = appController.EnrollmentRequestController;

            if (student == null) return null;
            List<ExamSlot> availableExams = new();

            List<EnrollmentRequest> studentRequests = enrollmentController.GetStudentRequests(student.Id);

            foreach (ExamSlot exam in GetAllExams().Values)
            {
                //don't include filled exams and exams that passed
                if (!IsAvailable(exam))
                {
                    continue;
                }

                foreach (EnrollmentRequest enrollmentRequest in studentRequests)
                {
                    Course course = courseController.GetById(enrollmentRequest.CourseId);
                    if (HasStudentAttendedCourse(course, enrollmentRequest, exam))
                    {
                        availableExams.Add(exam);
                    }
                }
            }

            return availableExams;
        }

        private bool HasStudentAttendedCourse(Course course, EnrollmentRequest enrollmentRequest, ExamSlot examSlot)
        {
            if (course.Language == examSlot.Language && course.Level == examSlot.Level)
            {
                if (enrollmentRequest.Status == Status.Accepted && course.IsCompleted())
                {
                    return true;
                }
            }
            return false;
        }


    }
}
