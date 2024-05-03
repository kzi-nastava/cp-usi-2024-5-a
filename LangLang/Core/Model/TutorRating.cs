using LangLang.Core.Repository.Serialization;

namespace LangLang.Core.Model
{
    public class TutorRating : ISerializable
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int StudentId {  get; set; }
        public int Rating { get; set; }

        public TutorRating() {}

        public TutorRating(int id, int tutorId, int studentId, int rating)
        {
            Id = id;
            TutorId = tutorId;
            StudentId = studentId;
            Rating = rating;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TutorId = int.Parse(values[1]);
            StudentId = int.Parse(values[2]);
            Rating = int.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                TutorId.ToString(),
                StudentId.ToString(),
                Rating.ToString()
            };
        }
    }
}
