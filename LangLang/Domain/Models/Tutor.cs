using LangLang.Domain.Attributes;
using LangLang.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel;
using LangLang.BusinessLogic.UseCases;

namespace LangLang.Domain.Models
{
    public class Tutor : IProfileHolder
    {

        [Show]
        [AllowCreate]
        [AllowUpdate]
        public Profile Profile { get; set; }
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
        public bool HasLanguageLevel(string language, Level level)
        {
            TutorSkillService service = new();
            List<LanguageLevel> skills = service.GetByTutor(this);
            foreach (var skill in skills)
            {
                if (skill.Language.Equals(language, StringComparison.OrdinalIgnoreCase) && skill.Level == level)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
