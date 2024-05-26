
using LangLang.Configuration;
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.BusinessLogic.UseCases
{
    public class ReportService
    {
        public Dictionary<Course, int> GetPenaltiesLastYear()
        {
            var penaltiesByCourse = new Dictionary<Course, int>();
            var penaltyService = new PenaltyPointService();
            var courseService = new CourseService();

            foreach (var course in courseService.GetCoursesHeldInLastYear())
            {
                int penaltyCount = penaltyService.CountByCourse(course);
                penaltiesByCourse.Add(course, penaltyCount);
            }

            return penaltiesByCourse;
        }

        public Dictionary<(Course, int), double> GetAverageGradeByPenaltyCount()
        {
            var courseService = new CourseService();
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
            var gradeService = new GradeService();
            var penaltyService = new PenaltyPointService();
            List<Student> students = penaltyService.GetStudentsByPenaltyCount(course, points);
            return gradeService.GetAverageGrade(students, course);
        }
    }
}
