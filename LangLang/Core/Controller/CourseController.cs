using LangLang.Core.Model;
using LangLang.Core.Model.DAO;
using LangLang.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Controller
{
    public class CourseController
    {
        private readonly CoursesDAO _courses;

        public CourseController()
        {
            _courses = new CoursesDAO();
        }

        public Dictionary<int, Course> GetAllCourses()
        {
            return _courses.GetAllCourses();
        }

        public Dictionary<int, Course> GetLiveCourses()
        {
            Dictionary<int, Course> courses = _courses.GetAllCourses();
            foreach (Course course in courses.Values)
            {
                if (course.Online)
                {
                    courses.Remove(course.Id);
                }
            }
            return courses;
        }

        public void Add(Course course)
        {
            _courses.AddCourse(course);
        }

        public void Update(Course course)
        {
            _courses.UpdateCourse(course);
        }

        public void Delete(int courseId)
        {
            _courses.RemoveCourse(courseId);
        }

        // Deletes all future courses made by tutor or updates all the active courses to have no tutor as well as future courses made by director
        public void DeleteCoursesByTutor(int tutorId)
        {
            foreach (Course course in GetCoursesByTutor(tutorId).Values)
            {
                if (course.StartDateTime > DateTime.Now)
                {
                    if (course.CreatedByDirector)
                    {
                        course.TutorId = -1;
                        _courses.UpdateCourse(course);
                    }
                    else
                    {
                        _courses.RemoveCourse(course.Id);
                    }
                }
                else
                {
                    course.TutorId = -1;
                    _courses.UpdateCourse(course);
                }
            }
        }

        public void Subscribe(IObserver observer)
        {
            _courses.Subscribe(observer);
        }
        // Method returns true if a new online course can be added
        // or false if there are time overlaps and the course cannot be created
        // The parameter exams is a list of ExamSlots of the same tutor who wants to create a new course
        public bool CanCreateOnlineCourse(Course course, List<ExamSlot> exams)
        {
            // Get the time and the date of the beginning and of the end of the new couse
            TimeSpan courseTime = course.StartDateTime.TimeOfDay;
            DateTime courseStartDate = course.StartDateTime.Date;
            DateTime courseEndDate = GetCourseEnd(course);

            // Go through each course with the same tutor and check if there is time overlapping 
            foreach (Course cour in GetCoursesByTutor(course.TutorId).Values)
            {
                // Check if the courses are overlapping
                DateTime courStartDate = cour.StartDateTime.Date;
                DateTime courEndDate = GetCourseEnd(cour);
                if (courStartDate <= courseEndDate && courEndDate >= courseStartDate)
                {
                    foreach (DayOfWeek day in cour.Days)
                    {
                        if (course.Days.Contains(day))
                        {
                            TimeSpan courTime = cour.StartDateTime.TimeOfDay;
                            TimeSpan difference = courseTime - courTime;
                            if (difference.Duration().TotalMinutes < 90)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // Go through all exams of the same tutor
            foreach (ExamSlot exam in exams)
            {
                DateTime examDate = exam.ExamDateTime.Date;
                if (examDate >= courseStartDate && examDate <= courseEndDate && course.Days.Contains(examDate.DayOfWeek))
                {
                    // Check if there are time overlaps
                    TimeSpan examTime = exam.ExamDateTime.TimeOfDay;
                    TimeSpan difference = courseTime - examTime;
                    if ((examTime < courseTime && difference.Duration().TotalMinutes < 240) || (examTime > courseTime && difference.Duration().TotalMinutes < 90))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Method returns true if a new live course can be added
        // or false if there are time overlaps and the course cannot be created
        // The parameter exams is a list of ExamSlots of all the exams
        public bool CanCreateLiveCourse(Course course, List<ExamSlot> exams)
        {
            // Get the time and the date of the beginning and of the end of the new couse
            TimeSpan courseTime = course.StartDateTime.TimeOfDay;
            DateTime courseStartDate = course.StartDateTime.Date;
            DateTime courseEndDate = GetCourseEnd(course);

            bool firstClassroomTaken = false;
            bool secondClassromTaken = false;
            // Go through each course with the same tutor or held in a classroom and check if there is time overlapping 
            foreach (Course cour in _courses.GetAllCourses().Values)
            {
                if(cour.TutorId != course.TutorId && cour.Online)
                {
                    continue;
                }
                // Check if the courses are overlapping
                DateTime courStartDate = cour.StartDateTime.Date;
                DateTime courEndDate = GetCourseEnd(cour);
                if (courStartDate <= courseEndDate && courEndDate >= courseStartDate)
                {
                    foreach (DayOfWeek day in cour.Days)
                    {
                        if (course.Days.Contains(day))
                        {
                            TimeSpan courTime = cour.StartDateTime.TimeOfDay;
                            TimeSpan difference = courseTime - courTime;
                            if (difference.Duration().TotalMinutes < 90)
                            {
                                if(firstClassroomTaken && secondClassromTaken)
                                {
                                    return false;
                                }
                                else if(firstClassroomTaken)
                                {
                                    secondClassromTaken = true;
                                }
                                else
                                {
                                    firstClassroomTaken = true;
                                }
                            }
                        }
                    }
                }
            }

            // Go through all exams of the same tutor
            foreach (ExamSlot exam in exams)
            {
                DateTime examDate = exam.ExamDateTime.Date;
                if (examDate >= courseStartDate && examDate <= courseEndDate && course.Days.Contains(examDate.DayOfWeek))
                {
                    // Check if there are time overlaps
                    TimeSpan examTime = exam.ExamDateTime.TimeOfDay;
                    TimeSpan difference = courseTime - examTime;
                    if ((examTime < courseTime && difference.Duration().TotalMinutes < 240) || (examTime > courseTime && difference.Duration().TotalMinutes < 90))
                    {
                        if (firstClassroomTaken && secondClassromTaken)
                        {
                            return false;
                        }
                        else if (firstClassroomTaken)
                        {
                            secondClassromTaken = true;
                        }
                        else
                        {
                            firstClassroomTaken = true;
                        }
                    }
                }
            }

            if (firstClassroomTaken && secondClassromTaken)
            {
                return false;
            }
            return true;
        }
        // Method checks if the course is valid for updating or canceling
        public bool IsCourseValid(int courseId)
        {
            Course course = GetAllCourses()[courseId];
            TimeSpan difference = course.StartDateTime - DateTime.Now;
            return difference.TotalDays < 7;
        }

        // Method checks if a certain course is available for the student
        public bool IsCourseAvailable(int courseId)
        {
            Course course = GetAllCourses()[courseId];
            TimeSpan difference = course.StartDateTime - DateTime.Now;
            if (difference.TotalDays > 7)
            {
                if (course.Online)
                {
                    return true;
                }
                else
                {
                    return course.NumberOfStudents <= course.MaxStudents;
                }
            }
            else
            {
                return false;
            }
        }

        // Returns all courses made by a certain tutor
        public Dictionary<int, Course> GetCoursesByTutor(Tutor tutor)
        {
            Dictionary<int, Course> coursesByTutor = new Dictionary<int, Course>();

            foreach (Course course in GetAllCourses().Values)
            {
                if (course.TutorId == tutor.Id)
                {
                    coursesByTutor[course.Id] = course;
                }
            }

            return coursesByTutor;
        }

        public Dictionary<int, Course> GetCoursesByTutor(int tutorId)
        {
            Dictionary<int, Course> coursesByTutor = new Dictionary<int, Course>();

            foreach (Course course in GetAllCourses().Values)
            {
                if (course.TutorId == tutorId)
                {
                    coursesByTutor[course.Id] = course;
                }
            }

            return coursesByTutor;
        }

        // Method returns DateTime of the beginning of the last class of the course
        public DateTime GetCourseEnd(Course course)
        {
            DateTime end = course.StartDateTime.AddDays(7 * course.NumberOfWeeks);
            DayOfWeek endDay = course.Days[course.Days.Count - 1];
            return end.AddDays((int)endDay - (int)end.DayOfWeek);
        }

        public Course GetById(int courseId)
        {
            return _courses.GetAllCourses()[courseId];
        }

        public List<Course> SearchCoursesByTutor(int tutorId, string language, LanguageLevel level, DateTime startDate, int duration, bool online)
        {
            List<Course> tutorsCourses = GetCoursesByTutor(tutorId).Values.ToList();
            return SearchCourses(tutorsCourses, language, level, startDate, duration, online);
        }

        public List<Course> SearchCourses(List<Course> searchableCourses, string language, LanguageLevel? level, DateTime startDate, int duration, bool? online)
        {
            List<Course> filteredCourses = searchableCourses.Where(course =>
            (language == "" || course.Language == language) && 
            (level == null || course.Level == level) &&
            (startDate == default || course.StartDateTime == startDate) &&
            (duration == 0 || course.NumberOfWeeks == duration) &&
            (online == false || course.Online == online)).ToList();

            return filteredCourses;
        }
    }   
}