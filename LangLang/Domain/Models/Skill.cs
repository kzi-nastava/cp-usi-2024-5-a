using LangLang.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Domain.Models
{
    public class Skill
    {
        private List<string> _language;
        private List<LanguageLevel> _level;

        public Skill()
        {
            _language = new List<string>();
            _level = new List<LanguageLevel>();
        }
        public Skill(List<string> language, List<LanguageLevel> level)
        {
            Language = language;
            Level = level;
        }

        public List<string> Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public List<LanguageLevel> Level
        {
            get { return _level; }
            set { _level = value; }
        }
    }
}
