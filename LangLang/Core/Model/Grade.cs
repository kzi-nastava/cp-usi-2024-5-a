using LangLang.Core.Repository.Serialization;

namespace LangLang.Core.Model
{
    public class Grade : ISerializable
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId {  get; set; }
        public int ActivityGrade {  get; set; }
        public int KnowledgeGrade {  get; set; }

        public Grade(int id, int courseId, int studentId, int activityGrade, int knowledgeGrade)
        {
            Id = id;
            CourseId = courseId;
            StudentId = studentId;
            ActivityGrade = activityGrade;
            KnowledgeGrade = knowledgeGrade;
        }

        public Grade() {}

        public string[] ToCSV()
        {
            return new string[] 
            {   
                Id.ToString(), 
                CourseId.ToString(), 
                StudentId.ToString(), 
                ActivityGrade.ToString(), 
                KnowledgeGrade.ToString() 
            };

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            CourseId = int.Parse(values[1]);
            StudentId = int.Parse(values[2]);
            ActivityGrade = int.Parse(values[3]);
            KnowledgeGrade = int.Parse(values[4]);   
        }
    }
}
