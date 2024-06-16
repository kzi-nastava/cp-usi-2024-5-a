
using LangLang.Configuration;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static int GetTutorForExam(ExamSlot exam)
        {
            TutorSkillService tutorSkillService = new();
            LanguageLevelService langLevelService = new();
            LanguageLevel skill = langLevelService.Get(exam.LanguageId);
            List<Tutor> tutors = tutorSkillService.GetBySkill(skill);
          
            if (tutors.Count == 0) return -1;
            TutorRatingService tutorRatingService = new();
            Dictionary<Tutor, double> tutorsAndRatings = new();
            foreach (Tutor tutor in tutors)
            {
                tutorsAndRatings[tutor] = tutorRatingService.GetAverageRating(tutor);
            }
            //sort by grades
            Dictionary<Tutor, double> sorted = tutorsAndRatings.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            ExamSlotService examSlotService = new();
            foreach (Tutor tutor in sorted.Keys)
            {
                exam.TutorId = tutor.Id;
                if (examSlotService.CanCreateExam(exam)) return tutor.Id;
            }
            return -1;
        }
        public static int MostSuitableTutor(Course course)
        {
            var courseService = new CourseService();
            var tutorSkillService = new TutorSkillService();
            var langLevelService = new LanguageLevelService();

            List<Tutor> tutors = new();
            LanguageLevel skill = langLevelService.Get(course.LanguageLevelId);
            tutors = tutorSkillService.GetBySkill(skill);

            //there is no free tutors for given course
            if (tutors.Count == 0) return -1;

            //consider only tutors that are free in time of course
            List<Tutor> availableTutors = new();
            foreach (Tutor tutor in tutors)
            {
                course.TutorId = tutor.Id;
                if (courseService.CanCreateOrUpdate(course)) {
                    availableTutors.Add(tutor); }
            }
            //find least busy tutors
            List<Tutor> leastBusyTutors = WithLeastActiveCourses(availableTutors);
            if(leastBusyTutors.Count == 0) return -1;
            //check who is rated the best
            return GetBestRankedTutor(leastBusyTutors);
        }
        private static int MinimumActiveCourses(List<Tutor> tutors)
        {
            CourseService coursesService = new();
            int minActive = int.MaxValue;
            int active = 0;
            foreach (var tutor in tutors)
            {
                active = coursesService.NumActiveCourses(tutor);
                if (active < minActive)
                {
                    minActive = active;
                }
            }

            return minActive;
        }
        private static List<Tutor> WithLeastActiveCourses(List<Tutor> tutors)
        {
            int minActive = MinimumActiveCourses(tutors);
            List<Tutor> leastBusyTutors = new List<Tutor>();
            CourseService coursesService = new();

            foreach (var tutor in tutors)
            {
                int active = coursesService.NumActiveCourses(tutor);
                if (active == minActive)
                {
                    leastBusyTutors.Add(tutor);
                }
            }

            return leastBusyTutors;
        }
        private static int GetBestRankedTutor(List<Tutor> tutors)
        {
            TutorRatingService tutorRatingService = new();
            Dictionary<Tutor, double> ratings = new();
            foreach (Tutor tutor in tutors)
            {
                ratings[tutor] = tutorRatingService.GetAverageRating(tutor);
            }
            Tutor bestRanked = ratings.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            return bestRanked.Id;
        }
        
    }
}
