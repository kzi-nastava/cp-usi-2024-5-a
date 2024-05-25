using System;
using LangLang.Domain.Enums;

namespace LangLang.Domain.Models
{
    public class Student : IProfileHolder
    {

        public Profile Profile { get; set; }
        public string Profession { get; set; }
        public int Id => Profile.Id;

        public Student(int id, string name, string lastName, Gender gender, DateTime dateOfBirth, string phoneNumber, string email, string password, UserType role, bool isActive, string profession)
        {
            Profile = new Profile(id, name, lastName, gender, dateOfBirth, phoneNumber, email, password, role, isActive);
            Profession = profession;
        }

        public Student(Profile profile, string profession)
        {
            Profile = profile;
            Profession = profession;
        }
    }
}
