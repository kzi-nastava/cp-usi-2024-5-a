
using LangLang.Configuration;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.BusinessLogic.UseCases
{
    public class SmartSystem
    {
        public static List<Student> GetTopStudents(Course course, bool knowledgePriority)
        {
            var penaltiesService = new PenaltyPointService();
            var studentService= new StudentService();
            var gradeService = new GradeService();
            Dictionary<int, Student> rankedStudents = new();

            foreach (var grade in gradeService.GetByCourse(course))
            {
                var student = studentService.Get(grade.StudentId);
                int penaltiesCount = penaltiesService.CountPenaltyPoints(student, course);

                int weightedGrade = GetWeightedGrade(grade, penaltiesCount, knowledgePriority);
                rankedStudents[weightedGrade] = student;
            }

            var sortedStudents = rankedStudents.OrderByDescending(kv => kv.Key);
            var topStudents = sortedStudents.Take(Constants.TOP_STUDENTS_COUNT).Select(kv => kv.Value).ToList();

            return topStudents;
        }

        private static int GetWeightedGrade(Grade grade, int penaltiesCount, bool knowledgePriority)
        {
            int w1 = knowledgePriority ? 2 : 1; // Weight for knowledge grade
            int w2 = knowledgePriority ? 1 : 2; // Weight for activity grade

            int weightedGrade = grade.KnowledgeGrade * w1 + grade.ActivityGrade * w2 - penaltiesCount;

            return weightedGrade;
        }
    }
}
