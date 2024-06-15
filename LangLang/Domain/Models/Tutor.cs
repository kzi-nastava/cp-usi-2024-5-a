using LangLang.ConsoleApp.Attributes;
using LangLang.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LangLang.Domain.Models
{
    public class Tutor : IProfileHolder
    {
        [Show]
        public Profile Profile { get; set; }

        public Skill Skill { get; set; }
        [Show]
        public DateTime EmploymentDate { get; set; }

        public int Id => Profile.Id;

        public Tutor(int id, string name, string lastName, Gender gender, DateTime birthDate, string phoneNumber, string email, string password, UserType role, bool isActive, DateTime employmentDate, List<string> languages, List<LanguageLevel> levels)
        {
            Profile = new(id, name, lastName, gender, birthDate, phoneNumber, email, password, role, isActive);
            EmploymentDate = employmentDate;
            Skill = new(languages, levels);
        }

        public Tutor()
        {
            Profile = new();
            Skill = new();
        }
        public bool HasLanguageLevel(string language, LanguageLevel level)
        {
            for (int i = 0; i < Skill.Language.Count; i++)
            {
                if (Skill.Language[i].Equals(language, StringComparison.OrdinalIgnoreCase) && Skill.Level[i] == level)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
