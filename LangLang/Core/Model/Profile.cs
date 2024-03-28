using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Model
{
    class Profile
    {
        // Attributes
        private string name;
        private string lastName;
        private UserGender gender;
        private DateTime dateOfBirth;
        private string phoneNumber;
        private string email;
        private string password;
        private UserType role;

        // Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public UserGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public UserType Role
        {
            get { return role; }
            set { role = value; }
        }

        // Constructor
        public Profile(string name, string lastName, UserGender gender, DateTime dateOfBirth, string phoneNumber, string email, string password, UserType role)
        {
            Name = name;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Email = email;
            Password = password;
            Role = role;
        }
        public Profile() { }

    }
}
