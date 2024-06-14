using System;
using LangLang.Configuration;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Linq;

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
                for (int i = 0; i <= Constants.MAX_PENALTY_POINTS; i++)
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

            if (results.Count == 0) return averages;

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
        public float CalculatePassingPercentage(Course course)
        {
            int attended = NumStudentsAttended(course);
            if (attended == 0) return 0;
            int passed = NumStudentsPassed(course);
            return (passed / attended)*100;
        }
        public Dictionary<string,float[]> GetCoursesAccomplishment()
        {
            Dictionary<string, float[]> accomplishments = new();
            CourseService courseService = new();
            List<Course> courses = courseService.GetCoursesHeldInLastYear();
            foreach (Course course in courses)
            {
                float[] res = new float[3];
                res[0] = NumStudentsAttended(course);
                res[1] = NumStudentsPassed(course);
                res[2] = CalculatePassingPercentage(course);
                accomplishments.Add(course.ToPdfString(), res);
            }
            return accomplishments;
        }
        public Dictionary<string, List<double>> GetAverageGradesOfCourses()
        {
            Dictionary<string, List<double>> averages = new();
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
                averages.Add(course.ToPdfString(), avg);
            }
            return averages;
        }


        // methods below for average penalty points
        public Dictionary<string, double> GetAveragePenaltyPoints()
        {
            var points = new Dictionary<string, double>();
            var courseService = new CourseService();
            foreach (string language in courseService.GetLanguages())
                points[language] = GetAveragePenaltyPoints(language);
            return points;
        }

        private double GetAveragePenaltyPoints(string language)
        {
            int points = 0;

            var courseService = new CourseService();
            var penaltyPointService = new PenaltyPointService();

            var courses = courseService.GetAll().Where(course => course.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            foreach (Course course in courses)
                points += penaltyPointService.GetByCourse(course).Count;

            return points;
        }

        // methods below for average points
        public Dictionary<string, double> GetAveragePoints()
        {
            var courseService = new CourseService();
            var points = new Dictionary<string, double>();
            foreach (string language in courseService.GetLanguages())
                points[language] = GetAveragePoints(language);
            return points;
        }

        private double GetAveragePoints(string language)
        {
            var examService = new ExamSlotService();
            var resultService = new ExamResultService();
            var exams = examService.GetByLanguage(language);

            int total = 0;
            int examinees = 0;

            foreach (ExamSlot exam in exams)
            {
                var results = resultService.GetByExam(exam);
                total += GetTotalPoints(results);
                examinees += results.Count;
            }

            return examinees == 0 ? 0 : (double)total / examinees;
        }

        private int GetTotalPoints(List<ExamResult> results)
        {
            int total = 0;
            foreach (ExamResult result in results)
                total += result.ListeningPoints + result.ReadingPoints + result.SpeakingPoints + result.WritingPoints;
            return total;
        }

        // methods below - number of courses created in the past year
        public Dictionary<string, double> GetNumberOfCourses()
        {
            var courses = new Dictionary<string, double>();
            var courseService = new CourseService();
            foreach (string language in courseService.GetLanguages())
                courses[language] = GetNumberOfCourses(language);
            return courses;
        }

        private int GetNumberOfCourses(string language)
        {
            var courseService = new CourseService();
            var courses = courseService.GetAll().Where(exam => exam.CreatedAt >= DateTime.Now.AddYears(-1) && exam.CreatedAt <= DateTime.Now);
            return courses.Count(course => course.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
        }

        // methods below - number of exams created in the past year

        public Dictionary<string, double> GetNumberOfExams()
        {
            var exams = new Dictionary<string, double>();
            var courseService = new CourseService();
            foreach (string language in courseService.GetLanguages())
                exams[language] = GetNumberOfExams(language);
            return exams;
        }

        private int GetNumberOfExams(string language)
        {
            var examService = new ExamSlotService();
            var languageService = new LanguageLevelService();

            var exams = examService.GetAll().Where(exam => languageService.Get(exam.LanguageId).Language.Equals(language, StringComparison.OrdinalIgnoreCase)).ToList();
            return exams.Count(exam => exam.CreatedAt >= DateTime.Now.AddYears(-1) && exam.CreatedAt <= DateTime.Now);
        }

    }
}
