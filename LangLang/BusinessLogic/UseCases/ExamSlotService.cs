using LangLang.Composition;
using LangLang.Configuration;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Update(ExamSlot exam)
        {
            _exams.Update(exam);
        }

        public void Add(ExamSlot exam)
        {
            if (!CanCreateExam(exam))
                throw new Exception("It's not possible to create exam, try entering different data");
            exam.Id = GenerateId();
            _exams.Add(exam);
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
            var resultService = new ExamResultService();
            var languageService = new LanguageLevelService();

            var exams = GetAll().Where(exam => languageService.Get(exam.LanguageId).Language.Equals(language, StringComparison.OrdinalIgnoreCase)).ToList();
            return exams.Where(exam => resultService.GetByExam(exam).All(result => result.Outcome != ExamOutcome.NotGraded && exam.ResultsGenerated)).ToList();
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
            var timeService = new TimeSlotService();

            foreach (Course course in courseService.GetAll())
            {
                //check for overlapping
                if (course.OverlappsWith(timeService.Get(exam.TimeSlotId)))
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
            var timeService = new TimeSlotService();

            foreach (ExamSlot currExam in GetAll())
            {
                if (currExam.Id == exam.Id)
                    continue;

                if (timeService.Get(exam.TimeSlotId).OverlappsWith(timeService.Get(currExam.TimeSlotId)))
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

        public void Delete(int id)
        {
            ExamSlot exam = Get(id);

            var timeService = new TimeSlotService();
            TimeSlot timeSlot = timeService.Get(exam.TimeSlotId);

            if (!((timeSlot.Time - DateTime.Now).TotalDays >= Constants.EXAM_MODIFY_PERIOD))
                throw new Exception("$Can't delete exam, there is less than {Constants.EXAM_MODIFY_PERIOD} days before exam.");
            
            _exams.Delete(exam);
        }

        public void DeleteByTutor(Tutor tutor)
        {
            var applicationService = new ExamApplicationService();
            var timeService = new TimeSlotService();
            
            foreach (ExamSlot exam in GetByTutor(tutor))
            {
                var timeSlot = timeService.Get(exam.TimeSlotId);

                if (timeSlot.Time > DateTime.Now)
                {
                    applicationService.DeleteByExam(exam);
                    Delete(exam.Id);
                }
            }
        }

        public void IncrementApplicants(ExamSlot exam)
        {           
            exam.Applicants++;
            _exams.Update(exam);
        }

        public void DecrementApplicants(ExamSlot exam)
        {
            exam.Applicants--;
            _exams.Update(exam);
        }

        public bool ApplicationsVisible(ExamSlot exam)
        {
            var timeSlotService = new TimeSlotService();
            TimeSlot timeSlot = timeSlotService.Get(exam.Id);

            int daysLeft = (timeSlot.Time - DateTime.Now).Days; // days left until exam
            double timeLeft = (timeSlot.GetEnd() - DateTime.Now).TotalMinutes; // time left until end of exam

            if (daysLeft > 0 && daysLeft < Constants.PRE_START_VIEW_PERIOD) return true; // applications become visible when there are less than PRE_START_VIEW_PERIOD days left
            else if (daysLeft == 0 && timeLeft > 0) return true; // on the exam day, applications are visible until the end of exam
            return false;
        }
        
        public bool CanBeUpdated(ExamSlot exam)
        {
            var timeService = new TimeSlotService();
            return (timeService.Get(exam.TimeSlotId).Time - DateTime.Now).TotalDays >= Constants.EXAM_MODIFY_PERIOD;
        }

        public List<ExamSlot> GetByTutor(Tutor tutor)
        {
            return _exams.GetByTutor(tutor);
        }

        public bool IsAvailable(ExamSlot exam)
        {
            if ( HasPassed(exam) || IsFullyBooked(exam) || IsLessThanMonthAway(exam) )
                return false;
            return true;
        }

        public List<ExamSlot> SearchByTutor(Tutor tutor, DateTime examDate, string language, Level? level)
        {
            return Search(GetByTutor(tutor), examDate, language, level);
        }

        public List<ExamSlot> SearchByStudent(Student student, DateTime examDate, string courseLanguage, Level? Level)
        {
            return Search(GetAvailableExams(student), examDate, courseLanguage, Level);
        }

        private List<ExamSlot> Search(List<ExamSlot> exams, DateTime examDate, string language, Level? level)
        {
            var languageService = new LanguageLevelService();
            var timeService = new TimeSlotService();

            return exams.Where(exam => (examDate == default || timeService.Get(exam.TimeSlotId).Time.Date == examDate.Date) &&
            (language == "" || languageService.Get(exam.LanguageId).Language == language) &&
            (level == null || languageService.Get(exam.LanguageId).Level == level)).ToList();
        }

        public bool HasPassed(ExamSlot exam)
        {
            var timeService = new TimeSlotService();
            return timeService.Get(exam.TimeSlotId).Time < DateTime.Now;
        }

        public bool IsFullyBooked(ExamSlot exam)
        {
            return exam.MaxStudents == exam.Applicants;
        }

        private bool IsLessThanMonthAway(ExamSlot exam)
        {
            var timeService = new TimeSlotService();
            return (timeService.Get(exam.TimeSlotId).Time - DateTime.Now).TotalDays < 30;
        }

        public List<ExamSlot> GetExamsHeldInLastYear()
        {
            return GetAll().Where(exam => IsHeldInLastYear(exam)).ToList();
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
                           .Where(exam => !applicationService.HasApplied(student, exam)) //exclude exams for which student has already applied
                           .Where(exam => !HasPassedLowerLevel(studentResults, exam))
                           .Where(exam => HasAttendedRequiredCourse(studentRequests, exam)).ToList();
            return exams;
        }

        private bool HasPassedLowerLevel(List<ExamResult> results, ExamSlot exam)
        {
            var examService = new ExamSlotService();
            var languageService = new LanguageLevelService();

            return results.Any(result => result.Outcome == ExamOutcome.Passed &&
                               languageService.Get(exam.LanguageId).Language == languageService.Get(examService.Get(result.ExamSlotId).LanguageId).Language &&
                               languageService.Get(exam.LanguageId).Level < languageService.Get(examService.Get(result.ExamSlotId).LanguageId).Level);
        }

        private bool HasAttendedRequiredCourse(List<EnrollmentRequest> requests, ExamSlot exam)
        {
            var courseService = new CourseService();
            return requests.Select(request => courseService.Get(request.CourseId))
           .Any(course => HasStudentAttendedCourse(course, exam));
        }

        private bool HasStudentAttendedCourse(Course course, ExamSlot examSlot)
        {
            var languageService = new LanguageLevelService();
            var examLanguage = languageService.Get(examSlot.LanguageId);
            var courseLanguage = languageService.Get(course.LanguageLevelId);
             
            return courseLanguage.Language == examLanguage.Language &&
               courseLanguage.Level <= examLanguage.Level && course.IsCompleted();
        }

        private bool IsHeldInLastYear(ExamSlot exam)
        {
            var timeService = new TimeSlotService();
            var timeSlot = timeService.Get(exam.TimeSlotId);

            return timeSlot.Time > DateTime.Now.AddYears(-1);
        }

    }
}
