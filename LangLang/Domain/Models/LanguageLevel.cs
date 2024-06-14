using LangLang.Domain.Enums;

namespace LangLang.Domain.Models
{
    public class LanguageLevel
    {
        public int Id { get; set; } 
        public string Language { get; set; }
        public Level Level { get; set; }

        public LanguageLevel() {}

        public LanguageLevel(int id, string language, Level level)
        {
            Id = id;
            Language = language;
            Level = level;
        }
    }
}
