using LangLang.Domain.Attributes;
using LangLang.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel;

namespace LangLang.Domain.Models
{
    public class Tutor : IProfileHolder
    {

        [Show]
        [AllowCreate]
        [AllowUpdate]
        public Profile Profile { get; set; }
        [AllowCreate]
        [AllowUpdate]
        public Skill Skill { get; set; }
        [Show]
        public DateTime EmploymentDate { get; set; }

        public int Id {  get; set; }

        public Tutor(string name, string lastName, Gender gender, DateTime birthDate, string phoneNumber, string email, string password, UserType role, bool isActive, DateTime employmentDate)
        {
            Profile = new(Id, name, lastName, gender, birthDate, phoneNumber, email, password, role, isActive);
            EmploymentDate = employmentDate;
        }

        public Tutor()
        {
            Profile = new();
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
