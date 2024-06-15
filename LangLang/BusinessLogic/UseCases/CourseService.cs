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
    public class CourseService
    {
        private ICourseRepository _courses;
        public CourseService()
        {
            _courses = Injector.CreateInstance<ICourseRepository>();
        }

        public List<Course> GetAll()
        {
            return _courses.GetAll();
        }

        public Course Get(int id)
        {
            return _courses.Get(id);
        }

        public void Add(Course course)
        {
            var courseTimeSlotService = new CourseTimeSlotService();
            _courses.Add(course);
            courseTimeSlotService.GenerateSlots(course);
        }

        public void Update(Course course)
        {
            _courses.Update(course);
        }

        public void Delete(Course course)
        {
            _courses.Delete(course);
        }

        public List<string> GetLanguages()
        {
            var languageService = new LanguageLevelService();
            return GetAll().Select(course => languageService.Get(course.LanguageLevelId).Language).Distinct().ToList();
        }

        public void DeleteByTutor(Tutor tutor)
        {
            foreach (Course course in GetByTutor(tutor.Id))
            {
                bool isFutureCourse = course.StartDateTime > DateTime.Now;
                bool isCreatedByDirector = course.CreatedByDirector;

                if (isFutureCourse && !isCreatedByDirector)
                {
                        EnrollmentRequestService enrollmentRequestService = new();
                        enrollmentRequestService.Delete(course);
                        Delete(course);
                } else {
                    course.TutorId = Constants.DELETED_TUTOR_ID;
                    Update(course);
                }
            }
        }

        public bool IsCompleted(int id)
        {
            Course course = Get(id) ?? throw new ArgumentException("There is no course with given id");
            return course.IsCompleted();
        }

        public bool CanCreateOrUpdate(Course course)
        {
            int busyClassrooms = 0;
            return !ExamsAndCourseOverlapp(course, ref busyClassrooms) && !CoursesOverlapp(course, ref busyClassrooms);
        }

        // Checks for any overlaps between exams and the course,
        // considering the availability of the courses's tutor and classrooms
        public bool ExamsAndCourseOverlapp(Course course, ref int busyClassrooms)
        {
            var examSlotService = new ExamSlotService();
            List<ExamSlot> examSlots = examSlotService.GetAll();
            // Go through exams
            //foreach (ExamSlot exam in examSlots)
            //{
            //    //check for overlapping
            //    if (course.OverlappsWith(exam.TimeSlot))
            //    {
            //        //tutor is busy (has exam)
            //        if (course.TutorId == exam.TutorId && course.TutorId != Constants.DELETED_TUTOR_ID) return true;

            //        if (!course.Online)
            //        {
            //            busyClassrooms++;
            //        }

            //        //all classrooms are busy
            //        if (busyClassrooms == 2) return true;
            //    }
            //}
            return false;
        }

        // Checks for any overlaps between other courses and current course,
        // considering the availability of the courses's tutor and classrooms
        public bool CoursesOverlapp(Course course, ref int busyClassrooms)
        {
            //foreach (Course currCourse in GetAll())
            //{
                // if this checks for updating course, then skip over the original course
                //if (currCourse.Id == course.Id) { continue; }

                //foreach (TimeSlot timeSlot in currCourse.TimeSlot)
                //{
                //    if (course.OverlappsWith(timeSlot))
                //    {
                //        // the tutor already has a course in one of the timeslots
                //        if (course.TutorId == currCourse.TutorId && course.TutorId != Constants.DELETED_TUTOR_ID) return true;

                //        //if the course or the currCourse is online continue,
                //        //because the classrooms do not matter
                //        if (course.Online || currCourse.Online) { continue; }

                //        busyClassrooms++;

                //        //all classrooms are busy
                //        if (busyClassrooms == 2) return true;
                //        {
                //            return true;
                //        }

                //    }
                //}
            //}
            return false;
        }

        public void IncrementStudents(int courseId)
        {
            var course = Get(courseId);
            course.NumberOfStudents++;
            Update(course);
        }

        public void DecrementStudents(int courseId)
        {
            var course = Get(courseId);
            course.NumberOfStudents--;
            Update(course);
        }

        // Method checks if a certain course is available for the student
        public bool IsAvailable(int courseId)
        {
            Course course = Get(courseId);
            TimeSpan difference = course.StartDateTime - DateTime.Now;
            if (difference.TotalDays > Constants.COURSE_AVAILABILITY_CHECK_PERIOD)
            {
                if (course.Online) return true;
                else return course.NumberOfStudents < course.MaxStudents;
            }
            return false;
        }

        // Method checks if the course is valid for updating or canceling
        public bool CanChange(int courseId)
        {
            Course course = Get(courseId);
            return course.StartDateTime >= DateTime.Now.AddDays(Constants.COURSE_MODIFY_PERIOD);
        }

        public int NumActiveCourses(Tutor tutor)
        {
            return GetByTutor(tutor.Id).Count(course => course.IsActive());
        }

        public List<Course> GetByTutor(int tutorId)
        {
            return GetAll().Where(course => course.TutorId == tutorId).ToList();
        }

    public List<Course> GetBySkills(Tutor tutor)
        {
            var tutorSkillService = new TutorSkillService();
            var languageService = new LanguageLevelService();

            var courses = new List<Course>();
            var availableCourses = GetAll();
            var tutorLanguage = tutorSkillService.GetByTutor(tutor.Id);

            foreach (var course in availableCourses)
            {
                var courseLanguage = languageService.Get(course.LanguageLevelId);
                foreach (LanguageLevel language in tutorLanguage)
                {
                    if (courseLanguage.Language == language.Language && courseLanguage.Level == language.Level)
                    {
                        courses.Add(course);
                        break;
                    }
                }
            }
            return courses;
        }

        public List<Course> GetAvailable(Student student)
        {
            var courseService = new CourseService();
            var enrollmentService = new EnrollmentRequestService();
            var availableCourses = new List<Course>();

            foreach (Course course in courseService.GetAll())
            {
                bool isAvailable = courseService.IsAvailable(course.Id);
                bool alreadyExists = enrollmentService.AlreadyExists(student, course);

                if (isAvailable && !alreadyExists)
                    availableCourses.Add(course);
            }
            return availableCourses;
        }

        public List<Course> GetCompleted(Student student)
        {
            var enrollmentService = new EnrollmentRequestService();
            var studentRequests = enrollmentService.GetByStudent(student);
            List<Course> courses = new();

            foreach (var request in studentRequests)
            {
                Course course = Get(request.CourseId);
                if (StudentAttendedUntilEnd(course, request))
                    courses.Add(course);
            }
            return courses;
        }

        private bool StudentAttendedUntilEnd(Course course, EnrollmentRequest request)
        {
            var withdrawalService = new WithdrawalRequestService();
            return course.IsCompleted() && request.Status == Status.Accepted
                    && !withdrawalService.HasAcceptedWithdrawal(request.Id);
        }

        public List<Course> SearchCoursesByTutor(int tutorId, string language, Level? level, DateTime startDate, int duration, bool? online)
        {
            return SearchCourses(GetByTutor(tutorId), language, level, startDate, duration, online);
        }

        public List<Course> SearchByStudent(Student student, string language, Level? level, DateTime startDate, int duration, bool? online)
        {
            return SearchCourses(GetAvailable(student), language, level, startDate, duration, online);
        }

        private List<Course> SearchCourses(List<Course> courses, string language, Level? level, DateTime startDate, int duration, bool? online)
        {
            var languageService = new LanguageLevelService();
            return courses.Where(course =>
            (language == "" || languageService.Get(course.LanguageLevelId).Language == language) &&
            (level == null || languageService.Get(course.LanguageLevelId).Level == level) &&
            (startDate == default || course.StartDateTime.Date == startDate.Date) &&
            (duration == 0 || course.NumberOfWeeks == duration) &&
            (online == false || course.Online == online)).ToList();
        }

        public List<Course> GetCoursesHeldInLastYear()
        {
            return GetAll().Where(course => course.IsHeldInLastYear()).ToList();
        }

        public List<Student> GetStudentsAttended(Course course)
        {
            List<Student> attended = new();
            EnrollmentRequestService enrollmentsService = new();
            List<EnrollmentRequest> enrollments = enrollmentsService.GetByCourse(course);
            StudentService studentsService = new();
            Student student;
            foreach(EnrollmentRequest request in enrollments)
            {
                if(StudentAttendedUntilEnd(course, request))
                {
                    student = studentsService.Get(request.StudentId);
                    attended.Add(student);
                }
            }
            return attended;
        }
        public int NumStudentsAttended(Course course)
        {
            return GetStudentsAttended(course).Count;
        }

        public int NumStudentsPassed(Course course)
        {
            return GetStudentsAttended(course).Count(student => HasStudentPassed(student, course));
        }

        public bool HasStudentPassed(Student student, Course course)
        {
            var resultsService = new ExamResultService();
            List<ExamResult> results = resultsService.GetByStudent(student);

            foreach (ExamResult result in results)
            {
                if (resultsService.IsResultForCourse(result, course) && result.Outcome == ExamOutcome.Passed)
                    return true;
            }
            return false;
        }

        public List<Course> GetGraded()
        {
            var gradeService = new GradeService();
            return GetAll().Where(course => gradeService.IsGraded(course) && !course.GratitudeEmailSent).ToList();
        }

    }
}
