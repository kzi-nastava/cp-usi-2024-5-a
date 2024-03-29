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
        private string _name;
        private string _lastName;
        private UserGender _gender;
        private DateTime _dateOfBirth;
        private string _phoneNumber;
        private string _email;
        private string _password;
        private UserType _role;

        // Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public UserGender Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public UserType Role
        {
            get { return _role; }
            set { _role = value; }
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

        public override string ToString()
        {
            return string.Join("|", new object[] { Name, LastName, Gender, DateOfBirth.ToString("yyyy-MM-dd"), PhoneNumber, Email, Password, Role });
        }

    }
}
