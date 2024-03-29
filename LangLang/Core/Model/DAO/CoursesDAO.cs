using System.Collections.Generic;
using System.Linq;
using LangLang.Core.Repository;
using LangLang.Core.Model;
using LangLang.Core.Observer;

namespace LangLang.Core.Model.DAO;

public class CoursesDAO : Subject
{
    private readonly List<Course> _courses;
    private readonly Repository<Course> _repository;

    public CoursesDAO()
    {
        _repository = new Repository<Course>("courses.csv");
        _courses = _repository.Load();
    }

    private int GenerateId()
    {
        if (_courses.Count == 0) return 0;
        return _courses.Last().Id + 1;
    }

    public Course AddCourse(Course Course)
    {
        Course.Id = GenerateId();
        _courses.Add(Course);
        _repository.Save(_courses);
        NotifyObservers();
        return Course;
    }

    public Course UpdateCourse(Course Course)
    {
        Course oldCourse = GetCourseById(Course.Id);
        if (oldCourse == null) return null;

        oldCourse.Language = Course.Language;
        oldCourse.Level = Course.Level;
        oldCourse.NumberOfWeeks = Course.NumberOfWeeks;
        oldCourse.Days = Course.Days;
        oldCourse.Online = Course.Online;
        oldCourse.NumberOfStudents = Course.NumberOfStudents;
        oldCourse.MaxStudents = Course.MaxStudents;
        
        _repository.Save(_courses);
        NotifyObservers();
        return oldCourse;
    }
    
    public Course RemoveCourse(int id)
    {
        Course course = GetCourseById(id);
        if (course == null) return null;

        _courses.Remove(course);
        _repository.Save(_courses);
        NotifyObservers();
        return course;
    }

    private Course GetCourseById(int id)
    {
        return _courses.Find(v => v.Id == id);
    }

    public List<Course> GetAllCourses()
    {
        return _courses;
    }

}