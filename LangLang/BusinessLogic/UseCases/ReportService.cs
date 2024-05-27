
using LangLang.Configuration;
using LangLang.Domain.Enums;
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

        public float[] GetAverageResults()
        {
            ExamSlotService examsService = new();
            ExamResultService resultsService = new();

            float[] averages = new float[] { 0f, 0f, 0f, 0f };
            List<ExamSlot> exams = examsService.GetExamsHeldInLastYear();
            List<ExamResult> results = new List<ExamResult>();
            results = resultsService.GetByExams(exams);

            foreach(ExamResult result in results)
            {
                averages[0] += result.ReadingPoints;
                averages[1] += result.SpeakingPoints;
                averages[2] += result.WritingPoints;
                averages[3] += result.ListeningPoints;
            }
            for (int i = 0; i < averages.Length; i++)
            {
                averages[i] /= results.Count;
            }
            return averages;
        }
        public int NumStudentsAttended(Course course)
        {
            CourseService coursesService = new();
            return coursesService.NumStudentsAttended(course);
        }
        public int NumStudentsPassed(Course course)
        {
            CourseService coursesService = new();
            return coursesService.NumStudentsPassed(course);
        }
        public double CalculatePassingPercentage(Course course)
        {
            int attended = NumStudentsAttended(course);
            int passed = NumStudentsPassed(course);
            return (passed / attended)*100;
        }

        public Dictionary<Course, List<double>> GetAverageGradesOfCourses()
        {
            Dictionary<Course, List<double>> averages = new();
            CourseService courseService = new();
            List<Course> courses = courseService.GetCoursesHeldInLastYear();
            GradeService gradeService = new();
            TutorRatingService tutorRatingService = new();  
            foreach (Course course in courses)
            {
                List<double> avg = new();
                avg.Add(gradeService.GetAverageKnowledgeGrade(course));
                avg.Add(gradeService.GetAverageActivityGrade(course));
                avg.Add(tutorRatingService.GetAverageTutorRating(course));
                averages.Add(course, avg);
            }
            return averages;
        }
    }
}
