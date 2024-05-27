using System;
using LangLang.Domain.Enums;

namespace LangLang.Domain.Models
{
    public class Director : IProfileHolder
    {
        public Profile Profile { get; set; }
        public int Id => Profile.Id;

        public Director(int id, string name, string lastName, Gender gender, DateTime birthDate, string phoneNumber, string email, string password, UserType role, bool isActive)
        {
            Profile = new(id, name, lastName, gender, birthDate, phoneNumber, email, password, role, isActive);
        }

        public Director() { }

    }
}
