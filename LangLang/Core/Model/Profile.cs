using System;

namespace LangLang.Core.Model
{
    // Classes containing an attribute of type Profile should implement this interface
    // for abstraction purposes over more complex user types
    public interface IProfileHolder
    {
        Profile Profile { get; }
    }

    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }    
        public UserType Role { get; set; }
        public bool IsDeleted { get; set; }

        public Profile(int id, string name, string lastName, Gender gender, DateTime birthDate, string phoneNumber, string email, string password, UserType role, bool isDeleted)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Gender = gender;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Email = email;
            Password = password;
            Role = role;
            IsDeleted = isDeleted;
        }

        public Profile() { }

        /// Constructor for initializing after parsing data loaded from file.
        /// <exception cref="FormatException">Thrown when date is not in the correct format.</exception>
        public Profile(string id, string name, string lastName, string gender, string birthDate, string phoneNumber, string email, string password, string role, string isDeleted)
        {
            try {
                BirthDate = DateTime.ParseExact(birthDate, Constants.DATE_FORMAT, null);
            } 
            catch {
                throw new FormatException("Date is not in the correct format.");
            }

            Id = int.Parse(id);
            Gender = (Gender)Enum.Parse(typeof(Gender), gender);
            Role = (UserType)Enum.Parse(typeof(UserType), role);
            Name = name; 
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email; 
            Password = password;
            IsDeleted = bool.Parse(isDeleted);
        }

        public override string ToString()
        {
            return string.Join("|", new object[] { Id, Name, LastName, Gender, BirthDate.ToString(Constants.DATE_FORMAT), PhoneNumber, Email, Password, Role, IsDeleted });
        }

    }
}
