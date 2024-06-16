﻿using System.Collections.Generic;
using System.ComponentModel;
using LangLang.Domain.Attributes;
using LangLang.Domain.Enums;

namespace LangLang.Domain.Models
{
    public class Skill
    {
        [Show]
        [AllowCreate]
        [AllowUpdate]
        [DisplayName("Languages")]
        public List<string> Language { get; set; }
        [Show]
        [AllowCreate]
        [AllowUpdate]
        [DisplayName("Respective levels")]
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
