using LangLang.Core.Model;
using System;
using System.Collections.Generic;

namespace LangLang.Domain.Models
{
    public class Tutor : IProfileHolder
    {

        public Profile Profile { get; set; }

        public Skill Skill { get; set; }

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

    }
}
