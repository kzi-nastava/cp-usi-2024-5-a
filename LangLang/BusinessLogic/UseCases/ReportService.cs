
using LangLang.Core;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.BusinessLogic.UseCases
{
    public class ReportService
    {
        public Dictionary<Course, int> GetPenaltiesLastYear()
        {
            var penaltiesByCourse = new Dictionary<Course, int>();
            var penaltyService = new PenaltyPointController();
            var courseService = new CourseController();

            foreach (var course in courseService.GetCoursesHeldInLastYear())
            {
                int penaltyCount = penaltyService.CountByCourse(course);
                penaltiesByCourse.Add(course, penaltyCount);
            }

            return penaltiesByCourse;
        }

        public Dictionary<(Course, int), double> GetAverageGradeByPenaltyCount()
        {
            var courseService = new CourseController();
            var averageGrades = new Dictionary<(Course, int), double>(); // key is pair (course, penaltyCount), value is avg grade

            foreach (var course in courseService.GetCoursesHeldInLastYear())
            {
                for (int i = 0; i < Constants.MAX_PENALTY_POINTS; i++)
                {
                    var key = (course, i);
                    averageGrades[key] = GetAverageGrade(course, i);
                }
            }

            return averageGrades;
        }

        private double GetAverageGrade(Course course, int points)
        {
            var gradeService = new GradeController();
            var penaltyService = new PenaltyPointController();
            List<Student> students = penaltyService.GetStudentsByPenaltyCount(course, points);
            return gradeService.GetAverageGrade(students, course);
        }
    }
}
