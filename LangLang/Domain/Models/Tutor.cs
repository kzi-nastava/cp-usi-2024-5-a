using LangLang.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LangLang.Domain.Models
{
    public class Tutor : IProfileHolder
    {
        private int _id;
        public Profile Profile { get; set; }

        public DateTime EmploymentDate { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id
        {
            get => Profile.Id;
            private set => _id = value;
        }

        public Tutor(int id, string name, string lastName, Gender gender, DateTime birthDate, string phoneNumber, string email, string password, UserType role, bool isActive, DateTime employmentDate)
        {
            Profile = new(id, name, lastName, gender, birthDate, phoneNumber, email, password, role, isActive);
            EmploymentDate = employmentDate;
        }

        public Tutor()
        {
            Profile = new();
        }

    }
}
