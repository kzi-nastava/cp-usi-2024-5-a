using System.Collections.Generic;
using LangLang.Domain.Enums;

namespace LangLang.Domain.Models
{
    public class Skill
    {
        public int Id { get; set; } 
        public string Language { get; set; }
        public LanguageLevel Level { get; set; }

        public Skill() {}

        public Skill(int id, string language, LanguageLevel level)
        {
            Id = id;
            Language = language;
            Level = level;
        }
    }
}
