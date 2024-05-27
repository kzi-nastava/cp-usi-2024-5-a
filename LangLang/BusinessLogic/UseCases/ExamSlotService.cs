using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Composition;
using LangLang.Domain.Enums;
using LangLang.Configuration;


namespace LangLang.BusinessLogic.UseCases
{
    public class ExamSlotService
    {
        private IExamSlotRepository _exams;

        public ExamSlotService()
        {
            _exams = Injector.CreateInstance<IExamSlotRepository>();
        }

        private int GenerateId()
        {
            var last = GetAll().LastOrDefault();
            return last?.Id + 1 ?? 0;
        }

        public ExamSlot? Get(int id)
        {
            return _exams.Get(id);
        }
        public List<ExamSlot> GetAll()
        {
            return _exams.GetAll();
        }

        public List<ExamSlot> GetGraded()
        {
            var gradedExams = new List<ExamSlot>();
            var resultsService = new ExamResultService();

            foreach (var exam in GetAll())
            {
                if (resultsService.GetByExam(exam).All(result => result.Outcome != ExamOutcome.NotGraded) && exam.ResultsGenerated && !exam.ExamineesNotified)
                    gradedExams.Add(exam);
            }

            return gradedExams;
        }

        // returns all graded exams for a specified language
        public List<ExamSlot> GetByLanguage(string language)
        {
            var exams = GetAll().Where(exam => exam.Language.Equals(language, StringComparison.OrdinalIgnoreCase)).ToList();
            var resultService = new ExamResultService();

            return exams.Where(exam => !resultService.GetByExam(exam).All(result => result.Outcome != ExamOutcome.NotGraded && exam.ResultsGenerated)).ToList();
        }

        //function takes examslot and adds it to dictionary of examslots
        //function saves changes and returns if adding was successful
        public bool Add(ExamSlot exam)
        {
            if (!CanCreateExam(exam)) return false;
            exam.Id = GenerateId();
            _exams.Add(exam);
            return true;

        }

        public bool CanCreateExam(ExamSlot exam)
        {
            int busyClassrooms = 0;
            return !CoursesAndExamOverlapp(exam, ref busyClassrooms) && !ExamsOverlapp(exam, ref busyClassrooms);
        }
        // Checks for any overlaps between courses and the exam, considering the availability of the exam's tutor and classrooms
        public bool CoursesAndExamOverlapp(ExamSlot exam, ref int busyClassrooms)
        {
            var courseService = new CourseService();
            List<Course> courses = courseService.GetAll().ToList();
            // Go through courses
            foreach (Course course in courses)
            {
                //check for overlapping
                if (course.OverlappsWith(exam.TimeSlot))
                {
                    //tutor is busy (has class)
                    if (course.TutorId == exam.TutorId)
                        return true;

                    if (!course.Online)
                        busyClassrooms++;

                    //all classrooms are busy
                    if (busyClassrooms == 2)
                        return true;

                }
            }
            return false;
        }
        // Checks for any overlaps between other exams and current exam, considering the availability of the exam's tutor and classrooms
        public bool ExamsOverlapp(ExamSlot exam, ref int busyClassrooms)
        {
            // Go through all exams
            foreach (ExamSlot currExam in GetAll())
            {
                if (currExam.Id == exam.Id)
                    continue;

                if (exam.TimeSlot.OverlappsWith(currExam.TimeSlot))
                {
                    //tutor is busy (has exam)
                    if (exam.TutorId == currExam.TutorId)
                        return true;

                    busyClassrooms++;

                    //all classrooms are busy
                    if (busyClassrooms == 2)
                        return true;
                }

            }
            return false;
        }

        //function takes id of examslot and removes examslot with that id
        //function saves changes and returns if removing was successful
        public bool Delete(int id)
        {
            ExamSlot exam = Get(id);
            if (exam == null) return false;

            if (!((exam.TimeSlot.Time - DateTime.Now).TotalDays >= Constants.EXAM_MODIFY_PERIOD)) return false;
            _exams.Delete(id);
            return true;

        }
        //deletes future exams by given tutor
        public void DeleteByTutor(Tutor tutor)
        {
            ExamApplicationService appsService = new();
            List<ExamSlot> exams = GetExams(tutor);
            foreach (ExamSlot exam in exams)
            {
                if (exam.TimeSlot.Time > DateTime.Now)
                {
                    appsService.DeleteByExam(exam);
                    Delete(exam.Id);
                }
            }
        }
        public void AddStudent(ExamSlot exam)
        {           
            exam.Applicants++;
            _exams.Update(exam);
        }

        public void RemoveStudent(ExamSlot exam)
        {
            exam.Applicants--;
            _exams.Update(exam);
        }
        
        public void Update(ExamSlot exam)
        {
            _exams.Update(exam);
        }

        public bool CanBeUpdated(ExamSlot exam)
        {
            return (exam.TimeSlot.Time - DateTime.Now).TotalDays >= Constants.EXAM_MODIFY_PERIOD;
        }


        // Method to get all exam slots by tutor ID
        //function takes tutor id
        public List<ExamSlot> GetExams(Tutor tutor)
        {
            return _exams.GetExams(tutor);
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

            if (IsLessThanMonthAway(exam))
            {
                return false;
            }

            return true;
        }

        public bool HasPassed(ExamSlot exam)
        {
            return exam.TimeSlot.Time < DateTime.Now;
        }

        public bool IsFullyBooked(ExamSlot exam)
        {
            return exam.MaxStudents == exam.Applicants;
        }

        // Check if exam is less than 30 days away
        private bool IsLessThanMonthAway(ExamSlot exam)
        {
            DateTime currentDate = DateTime.Now;

            TimeSpan difference = exam.TimeSlot.Time - currentDate;

            return difference.TotalDays < 30;
        }
        // Method to search exam slots by tutor and criteria
        public List<ExamSlot> SearchByTutor(Tutor tutor, DateTime examDate, string language, LanguageLevel? level)
        {
            List<ExamSlot> exams = _exams.GetAll();

            exams = GetExams(tutor);

            return Search(exams, examDate, language, level);
        }

        private List<ExamSlot> Search(List<ExamSlot> exams, DateTime examDate, string language, LanguageLevel? level)
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
            ExamSlot examSlot = Get(id);
            return examSlot.ApplicationsVisible();
        }

        public List<ExamSlot> SearchByStudent(Student student, DateTime examDate, string courseLanguage, LanguageLevel? languageLevel)
        {
            List<ExamSlot> availableExamSlots = GetAvailableExams(student);
            return Search(availableExamSlots, examDate, courseLanguage, languageLevel);
        }


        public List<ExamSlot> GetAvailableExams(Student student)
        {
            List<ExamSlot> availableExams = new();

            var enrollmentReqService = new EnrollmentRequestService();
            var resultService = new ExamResultService();
            var applicationService = new ExamApplicationService();
            var examService = new ExamSlotService();
            var courseService = new CourseService();


            List<EnrollmentRequest> studentRequests = enrollmentReqService.GetByStudent(student);
            List<ExamResult> studentResults = resultService.GetByStudent(student);

            var exams = GetAll().Where(exam => IsAvailable(exam))  // exclude exams that have been filled, passed, or are less than a month away.
                           .Where(exam => applicationService.HasApplied(student, exam)) //exclude exams for which student has already applied
                           .Where(exam => !HasPassedLowerLevel(studentResults, exam))
                           .Where(exam => HasAttendedRequiredCourse(studentRequests, exam)).ToList();
            return exams;
        }

        private bool HasPassedLowerLevel(List<ExamResult> results, ExamSlot exam)
        {
            var examService = new ExamSlotService();
            return results.Any(result => result.Outcome == ExamOutcome.Passed &&
                               exam.Language == examService.Get(result.ExamSlotId).Language &&
                               exam.Level < examService.Get(result.ExamSlotId).Level);
        }

        private bool HasAttendedRequiredCourse(List<EnrollmentRequest> requests, ExamSlot exam)
        {
            var courseService = new CourseService();
            return requests.Select(request => courseService.Get(request.CourseId))
           .Any(course => HasStudentAttendedCourse(course, exam));
        }

        private bool HasStudentAttendedCourse(Course course, ExamSlot examSlot)
        {
            return course.Language == examSlot.Language &&
               course.Level == examSlot.Level &&
               course.IsCompleted();
        }

        public List<ExamSlot> GetExamsHeldInLastYear()
        {
            return GetAll().Where(exam => exam.IsHeldInLastYear()).ToList();
        }


    }
}
