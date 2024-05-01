using LangLang.Core.Model;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;


namespace LangLang.DTO
{
    public class StudentDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        public StudentDTO() { }
        public int Id { get; set; }

        private string name;
        private string lastName;
        private string email;
        private string password;
        private string profession;
        private string phoneNumber;
        private Gender gender;
        private DateTime birthDate;


        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (value != lastName)
                {
                    lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (value != email)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (value != password)
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public string Profession
        {
            get { return profession; }
            set
            {
                if (value != profession)
                {
                    profession = value;
                    OnPropertyChanged("Profession");
                }
            }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (value != phoneNumber)
                {
                    phoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }

        public Gender Gender
        {
            get { return gender; }
            set
            {
                if (value != gender)
                {
                    gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                if (value != birthDate)
                {
                    birthDate = value;
                    OnPropertyChanged("BirthDate");
                }
            }
        }

        private readonly Regex _NumberRegex = new("^\\+?\\d+$");
        private readonly Regex _EmailRegex = new("^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,}$");
        private readonly Regex _NameRegex = new("^[A-Za-z\\-]+$");
        private readonly Regex _PasswordRegex = new("^(?=.*[0-9]).{8,}$");
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name)) return "Name is required";
                    if (!_NameRegex.Match(Name).Success) return "Name consists of letters and may contain hyphens (-).";
                    else return "";
                }

                if (columnName == "LastName")
                {
                    if (string.IsNullOrEmpty(LastName)) return "Lastname is required";
                    if (!_NameRegex.Match(LastName).Success) return "Last name consists of letters and may contain hyphens (-).";
                    else return "";
                }

                if (columnName == "Email")
                {
                    if (string.IsNullOrEmpty(email)) return "Email is required";
                    if (!_EmailRegex.Match(email).Success) return "Email contains @ and domain (example@example.com)";
                    else return "";
                }

                if (columnName == "Password")
                {
                    if (string.IsNullOrEmpty(password)) return "Password is required.";
                    if (!_PasswordRegex.Match(password).Success) return "Password contains at least 8 characters, with at least one number";
                    else return "";
                }
                if (columnName == "PhoneNumber")
                {
                    if (string.IsNullOrEmpty(phoneNumber)) return "Phone number is required.";
                    if (!_NumberRegex.Match(phoneNumber).Success) return "Phone number consists of digits and may start with a plus sign (+).";
                    else return "";
                }
                if (columnName == "BirthDate")
                {
                    if (birthDate > DateTime.Now) return "Please enter a valid date. Dates in the future are not allowed.";
                    if (birthDate == default) return "Birth date is required";
                    else return "";
                }

                return "";
            }
        }

        private readonly string[] _validatedProperties = { "Name", "LastName", "Email", "Profession", "Password", "PhoneNumber", "Gender", "BirthDate" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != "")
                        return false;
                }
                return true;

            }
        }

        public Student ToStudent()
        {
            return new Student(Id, name, lastName, gender, birthDate, phoneNumber, email, password, UserType.Student, false, profession);
        }

        public StudentDTO(Student student)
        {
            Id = student.Id;
            name = student.Profile.Name;
            lastName = student.Profile.LastName;
            gender = student.Profile.Gender;
            email = student.Profile.Email;
            password = student.Profile.Password;
            birthDate = student.Profile.BirthDate;
            phoneNumber = student.Profile.PhoneNumber;
            profession = student.Profession;
        }

        public string Error => throw new NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
