using LangLang.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LangLang.Domain.Models
{
    public class Tutor : IProfileHolder
    {
        public Profile Profile { get; set; }

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

    }
}
