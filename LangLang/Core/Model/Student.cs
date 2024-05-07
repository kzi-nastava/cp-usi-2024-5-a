using LangLang.Core.Controller;
using LangLang.Core.Repository.Serialization;
using System;

namespace LangLang.Core.Model
{
    public class Student : ISerializable, IProfileHolder
    {

        public Profile Profile { get; set; }
        public string Profession { get; set; }
        public int Id => Profile.Id;

        public Student() { }

        public Student(int id, string name, string lastName, Gender gender, DateTime dateOfBirth, string phoneNumber, string email, string password, UserType role,bool isActive ,string profession)
        {
            Profile = new Profile(id, name, lastName, gender, dateOfBirth, phoneNumber, email, password, role, isActive);
            Profession = profession;
        }

        public void FromCSV(string[] values) 
        {
            Profile = new Profile(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9]);
            Profession = values[10];
        }

        public string[] ToCSV()
        {
            return new string[] {
                Profile.ToString(),
                Profession
            };
        }
    }
}
