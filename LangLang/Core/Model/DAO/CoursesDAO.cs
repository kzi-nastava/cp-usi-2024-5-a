using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository;
using LangLang.Core.Observer;
using System;
using LangLang.Core.Controller;
using System.Windows.Ink;
using System.Windows.Input;
using LangLang.Core.Model.Enums;
using LangLang.Domain.Models;
using LangLang.BusinessLogic.UseCases;

namespace LangLang.Core.Model.DAO;

public class CoursesDAO : Subject
{
    private readonly Dictionary<int, Course> _courses;
    private readonly Repository<Course> _repository;

    public CoursesDAO()
    {
        _repository = new Repository<Course>("courses.csv");
        _courses = _repository.Load();
    }

    private int GenerateId()
    {
        if (_courses.Count == 0) return 0;
        return _courses.Keys.Max() + 1;
    }

    public Course Add(Course course)
    {
        course.Id = GenerateId();
        _courses[course.Id] = course;
        _repository.Save(_courses);
        NotifyObservers();
        return course;
    }

    public Course Update(Course course)
    {
        Course oldCourse = Get(course.Id);
        if (oldCourse == null) return null;

        oldCourse.Language = course.Language;
        oldCourse.Level = course.Level;
        oldCourse.NumberOfWeeks = course.NumberOfWeeks;
        oldCourse.Days = course.Days;
        oldCourse.Online = course.Online;
        oldCourse.NumberOfStudents = course.NumberOfStudents;
        oldCourse.MaxStudents = course.MaxStudents;
        oldCourse.StartDateTime = course.StartDateTime;
        oldCourse.TutorId = course.TutorId;
        oldCourse.Modifiable = course.Modifiable;

        _repository.Save(_courses);
        NotifyObservers();
        return oldCourse;
    }
    
    public Course Remove(int id)
    {
        Course course = Get(id);
        if (course == null) return null;

        _courses.Remove(id);
        _repository.Save(_courses);
        NotifyObservers();
        return course;
    }

    private Course Get(int id)
    {
        return _courses[id];
    }

    public Dictionary<int, Course> GetAll()
    {
        return _courses;
    }

    // Deletes all future courses made by tutor or updates all the active courses to have no tutor as well as future courses made by director
    public void DeleteByTutor(Tutor tutor)
    {
        foreach (Course course in GetByTutor(tutor))
        {
            if (course.StartDateTime > DateTime.Now)
            {
                if (course.CreatedByDirector)
                {
                    course.TutorId = -1;
                    Update(course);
                }
                else
                {
                    Remove(course.Id);
                }
            }
            else
            {
                course.TutorId = -1;
                Update(course);
            }
        }
    }

    public bool IsCompleted(int id)
    {
        Course course = Get(id) ?? throw new ArgumentException("There is no course with given id");
        return course.IsCompleted();
    }

    public bool CanCreateOrUpdate(Course course, ExamSlotController examSlotController)
    {
        int busyClassrooms = 0;
        return !ExamsAndCourseOverlapp(course, examSlotController, ref busyClassrooms) && !CoursesOverlapp(course, ref busyClassrooms);
    }

    // Checks for any overlaps between exams and the course,
    // considering the availability of the courses's tutor and classrooms
    public bool ExamsAndCourseOverlapp(Course course, ExamSlotController examSlotController, ref int busyClassrooms)
    {
        List<ExamSlot> examSlots = examSlotController.GetAll();
        // Go through exams
        foreach (ExamSlot exam in examSlots)
        {
            //check for overlapping
            if (course.OverlappsWith(exam.TimeSlot))
            {
                //tutor is busy (has exam)
                if (course.TutorId == exam.TutorId) return true;

                if (!course.Online)
                {
                    busyClassrooms++;
                }

                //all classrooms are busy
                if (busyClassrooms == 2) return true;
            }
        }
        return false;
    }
    // Checks for any overlaps between other courses and current course,
    // considering the availability of the courses's tutor and classrooms
    public bool CoursesOverlapp(Course course, ref int busyClassrooms)
    {
        foreach (Course currCourse in GetAll().Values)
        {
            // if this checks for updating course, then skip over the original course
            if(currCourse.Id == course.Id) { continue; }

            foreach(TimeSlot timeSlot in currCourse.TimeSlots)
            {
                if (course.OverlappsWith(timeSlot))
                {
                    // the tutor already has a course in one of the timeslots
                    if (course.TutorId == currCourse.TutorId) return true;

                    //if the course or the currCourse is online continue,
                    //because the classrooms do not matter
                    if (course.Online || currCourse.Online) { continue; }

                    busyClassrooms++;

                    //all classrooms are busy
                    if (busyClassrooms == 2) return true;
                    {
                        return true;
                    }

                }
            }
        }
        return false;
    }

    public void AddStudent(int courseId)
    {
        Course course = Get(courseId);
        course.NumberOfStudents += 1;
        Update(course);
    }

    public void RemoveStudent(int courseId)
    {
        Course course = Get(courseId);
        course.NumberOfStudents -= 1;
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
        DateTime oneWeekFromNow = DateTime.Now.AddDays(Constants.COURSE_MODIFY_PERIOD);
        return course.StartDateTime >= oneWeekFromNow;
    }

    public DateTime GetEnd(Course course)
    {
        return course.TimeSlots[course.TimeSlots.Count-1].GetEnd();
    }

    public bool IsActive(Course course)
    {
        if (course.StartDateTime <= DateTime.Now && GetEnd(course) >= DateTime.Now) return true;
        return false;
    }

    public List<DateTime> CalculateClassDates(DateTime startDate, DateTime endDate, List<DayOfWeek> weekdays, TimeSpan classTime)
    {
        List<DateTime> classDates = new List<DateTime>();


        return classDates;
    }

    public List<Course> GetByTutor(Tutor tutor)
    {
        List<Course> coursesByTutor = new List<Course>();

        foreach (Course course in _courses.Values)
        {
            if (course.TutorId == tutor.Id)
            {
                coursesByTutor.Add(course);
            }
        }

        return coursesByTutor;
    }

    public List<Course> GetByTutor(int tutorId)
    {
        List<Course> coursesByTutor = new List<Course>();

        foreach (Course course in _courses.Values)
        {
            if (course.TutorId == tutorId)
            {
                coursesByTutor.Add(course);
            }
        }

        return coursesByTutor;
    }

    public List<Course> GetBySkills(Tutor tutor)
    {
        List<Course> skills = new List<Course>();
        Dictionary<int, Course> courses = _courses;

        for (int i = 0; i < tutor.Skill.Language.Count; i++)
        {
            foreach (Course course in courses.Values)
            {
                if (course.Language == tutor.Skill.Language[i])
                {
                    if (course.Level == tutor.Skill.Level[i])
                    {
                        skills.Add(course);
                        break;
                    }
                }
            }
        }
        
        return skills;
    }

    public List<Course> SearchCoursesByTutor(int tutorId, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online)
    {
        List<Course> tutorsCourses = GetByTutor(tutorId);
        return SearchCourses(tutorsCourses, language, level, startDate, duration, online);
    }

    private List<Course> SearchCourses(List<Course> searchableCourses, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online)
    {
        List<Course> filteredCourses = searchableCourses.Where(course =>
        (language == "" || course.Language.Contains(language)) &&
        (level == null || course.Level == level) &&
        (startDate == default || course.StartDateTime.Date == startDate.Date) &&
        (duration == 0 || course.NumberOfWeeks == duration) &&
        (online == false || course.Online == online)).ToList();

        return filteredCourses;
    }

    public List<Course> SearchCoursesByStudent(Student student, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online)
    {
        List<Course> availableCourses = GetAvailable(student);
        List<Course> filteredCourses = SearchCourses(availableCourses, language, level, startDate, duration, online);
        return filteredCourses;
    }

    public List<Course> GetAvailable(Student student)
    {
        var courseService = new CourseController();
        var enrollmentService = new EnrollmentRequestService();

        List<Course> availableCourses = new();
        foreach (Course course in courseService.GetAll().Values)
        {
            if (courseService.IsAvailable(course.Id))
            {
                if (!enrollmentService.AlreadyExists(student, course))
                    availableCourses.Add(course);
            }
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


}