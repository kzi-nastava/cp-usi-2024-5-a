using System.Collections.Generic;
using LangLang.ConsoleApp.Attributes;
using LangLang.Domain.Enums;

namespace LangLang.Domain.Models
{
    public class Skill
    {
        [Show]
        public List<string> Language { get; set; }
        [Show]
        public List<LanguageLevel> Level { get; set; }

        public Skill()
        {
            Language = new List<string>();
            Level = new List<LanguageLevel>();
        }
        public Skill(List<string> language, List<LanguageLevel> level)
        {
            Language = language;
            Level = level;
        }

    }
}
