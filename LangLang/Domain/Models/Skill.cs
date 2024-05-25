﻿using LangLang.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Core.Model;

namespace LangLang.Domain.Models
{
    public class Skill
    {
        public List<string> Language { get; set; }
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
