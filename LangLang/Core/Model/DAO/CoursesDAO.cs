using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository;
using LangLang.Core.Model;
using LangLang.Core.Observer;

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

}