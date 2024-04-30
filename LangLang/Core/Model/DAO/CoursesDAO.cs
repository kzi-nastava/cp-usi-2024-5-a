using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository;
using LangLang.Core.Model;
using LangLang.Core.Observer;
using System;

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
        if (difference.TotalDays > 7)
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
        DateTime oneWeekFromNow = DateTime.Now.AddDays(7);
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
}