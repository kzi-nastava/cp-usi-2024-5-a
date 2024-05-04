using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using System;
using LangLang.Core.Controller;
using System.Windows.Ink;
using System.Windows.Input;

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

    public Course AddCourse(Course course)
    {
        course.Id = GenerateId();
        _courses[course.Id] = course;
        _repository.Save(_courses);
        NotifyObservers();
        return course;
    }

    public Course UpdateCourse(Course course)
    {
        Course oldCourse = GetCourseById(course.Id);
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

        _repository.Save(_courses);
        NotifyObservers();
        return oldCourse;
    }
    
    public Course RemoveCourse(int id)
    {
        Course course = GetCourseById(id);
        if (course == null) return null;

        _courses.Remove(id);
        _repository.Save(_courses);
        NotifyObservers();
        return course;
    }

    private Course GetCourseById(int id)
    {
        return _courses[id];
    }

    public Dictionary<int, Course> GetAllCourses()
    {
        return _courses;
    }

    // Deletes all future courses made by tutor or updates all the active courses to have no tutor as well as future courses made by director
    public void DeleteCoursesWithTutor(int tutorId)
    {
        foreach (Course course in GetCoursesWithTutor(tutorId).Values)
        {
            if (course.StartDateTime > DateTime.Now)
            {
                if (course.CreatedByDirector)
                {
                    course.TutorId = -1;
                    UpdateCourse(course);
                }
                else
                {
                    RemoveCourse(course.Id);
                }
            }
            else
            {
                course.TutorId = -1;
                UpdateCourse(course);
            }
        }
    }

    public bool IsCompleted(int id)
    {
        Course course = GetCourseById(id) ?? throw new ArgumentException("There is no course with given id");
        return course.IsCompleted();
    }

    public bool CanCreateOrUpdateCourse(Course course, ExamSlotController examSlotController)
    {
        int busyClassrooms = 0;
        return !ExamsAndCourseOverlapp(course, examSlotController, ref busyClassrooms) && !CoursesOverlapp(course, ref busyClassrooms);
    }

    // Checks for any overlaps between exams and the course,
    // considering the availability of the courses's tutor and classrooms
    public bool ExamsAndCourseOverlapp(Course course, ExamSlotController examSlotController, ref int busyClassrooms)
    {
        List<ExamSlot> examSlots = examSlotController.GetAllExams();
        // Go through exams
        foreach (ExamSlot exam in examSlots)
        {
            //check for overlapping
            if (course.OverlappsWith(exam.TimeSlot))
            {
                //tutor is busy (has exam)
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
    // Checks for any overlaps between other courses and current course,
    // considering the availability of the courses's tutor and classrooms
    public bool CoursesOverlapp(Course course, ref int busyClassrooms)
    {
        foreach (Course currCourse in GetAllCourses().Values)
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
                    if (busyClassrooms == 2)
                    {
                        return true;
                    }

                }
            }
        }
        return false;
    }

    public void AddStudentToCourse(int courseId)
    {
        Course course = GetCourseById(courseId);
        course.NumberOfStudents += 1;
        UpdateCourse(course);
    }

    // Method checks if a certain course is available for the student
    public bool IsCourseAvailable(int courseId)
    {
        Course course = GetCourseById(courseId);
        TimeSpan difference = course.StartDateTime - DateTime.Now;
        if (difference.TotalDays > Constants.COURSE_AVAILABILITY_CHECK_PERIOD)
        {
            if (course.Online) return true;
            else return course.NumberOfStudents < course.MaxStudents;
        }
        return false;
    }

    // Method checks if the course is valid for updating or canceling
    public bool CanCourseBeChanged(int courseId)
    {
        Course course = GetCourseById(courseId);
        DateTime oneWeekFromNow = DateTime.Now.AddDays(Constants.COURSE_MODIFY_PERIOD);
        return course.StartDateTime >= oneWeekFromNow;
    }

    public DateTime GetCourseEnd(Course course)
    {
        return course.TimeSlots[course.TimeSlots.Count-1].GetEnd();
    }

    public List<DateTime> CalculateClassDates(DateTime startDate, DateTime endDate, List<DayOfWeek> weekdays, TimeSpan classTime)
    {
        List<DateTime> classDates = new List<DateTime>();


        return classDates;
    }

    public Dictionary<int, Course> GetCoursesWithTutor(Tutor tutor)
    {
        Dictionary<int, Course> coursesByTutor = new Dictionary<int, Course>();

        foreach (Course course in _courses.Values)
        {
            if (course.TutorId == tutor.Id)
            {
                coursesByTutor[course.Id] = course;
            }
        }

        return coursesByTutor;
    }

    public Dictionary<int, Course> GetCoursesWithTutor(int tutorId)
    {
        Dictionary<int, Course> coursesByTutor = new Dictionary<int, Course>();

        foreach (Course course in _courses.Values)
        {
            if (course.TutorId == tutorId)
            {
                coursesByTutor[course.Id] = course;
            }
        }

        return coursesByTutor;
    }

    public Dictionary<int, Course> GetLiveCourses()
    {
        Dictionary<int, Course> courses = _courses;
        foreach (Course course in courses.Values)
        {
            if (course.Online)
            {
                courses.Remove(course.Id);
            }
        }
        return courses;
    }
    public List<Course> GetCoursesForSkills(Tutor tutor)
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
        List<Course> tutorsCourses = GetCoursesWithTutor(tutorId).Values.ToList();
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

    public List<Course> SearchCoursesByStudent(AppController appController, Student student, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online)
    {
        List<Course> availableCourses = GetAvailableCourses(student, appController);
        List<Course> filteredCourses = SearchCourses(availableCourses, language, level, startDate, duration, online);
        return filteredCourses;
    }

    public List<Course> GetAvailableCourses(Student student, AppController appController)
    {
        var courseController = appController.CourseController;
        var enrollmentController = appController.EnrollmentRequestController;

        List<Course> availableCourses = new();
        foreach (Course course in courseController.GetAllCourses().Values)
        {
            if (courseController.IsCourseAvailable(course.Id))
            {
                if (!enrollmentController.IsRequestDuplicate(student.Id, course))
                    availableCourses.Add(course);
            }
        }
        return availableCourses;
    }

    
}