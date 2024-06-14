
namespace LangLang.Domain.Models
{
    public class TutorSkill
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int LanguageLevelId { get; set; }

        public TutorSkill() { }

        public TutorSkill(int id, int tutorId, int languageLevelId)
        {
            Id = id;
            TutorId = tutorId;
            LanguageLevelId = languageLevelId;
        }
    }
}
