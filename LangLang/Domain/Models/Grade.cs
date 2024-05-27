
namespace LangLang.Domain.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public int ActivityGrade { get; set; }
        public int KnowledgeGrade { get; set; }

        public Grade(int id, int courseId, int studentId, int activityGrade, int knowledgeGrade)
        {
            Id = id;
            CourseId = courseId;
            StudentId = studentId;
            ActivityGrade = activityGrade;
            KnowledgeGrade = knowledgeGrade;
        }

        public Grade() { }

        public override string ToString()
        {
            return string.Join("|", new object[] { Id, CourseId, StudentId, ActivityGrade, KnowledgeGrade});
        }
    }
}
