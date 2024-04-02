using LangLang.Core.Model;
using System.ComponentModel;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LangLang.View.DTO
{
    public class TutorDTO : INotifyPropertyChanged, IDataErrorInfo
    {

        public int Id { get; set; }

        private string name;
        private string lastName;
        private UserGender gender;
        private DateTime birthDate;
        private string phoneNumber;
        private string email;
        private string password;
        private UserType role;
        private DateTime employmentDate;
        private List<string> languages;
        private List<LanguageLevel> levels;
        private List<string>  languagelevel;

        public string Name
        {
            get
            {
                return name;
            }
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
            get
            {
                return lastName;
            }
            set
            {
                if (value != lastName)
                {
                    lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        public UserGender Gender
        {
            get
            {
                return gender;
            }
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
            get
            {
                return birthDate;
            }

            set
            {
                if (value != birthDate)
                {
                    birthDate = value;
                    OnPropertyChanged("BirthDate");
                }
            }
        }

        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }

            set
            {
                if (value != phoneNumber)
                {
                    phoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

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
            get
            {
                return password;
            }

            set
            {
                if (value != password)
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public DateTime EmploymentDate
        {
            get
            {
                return employmentDate;
            }
            set
            {
                if (value != employmentDate)
                {
                    employmentDate = value;
                    OnPropertyChanged("EmploymentDate");
                }
            }
        }

        public UserType Role
        {
            get
            {
                return role;
            }

            set
            {
                if (role != value)
                {
                    role = value;
                    OnPropertyChanged("Role");
                }
            }
        }

       public List<String> Language
        {
            get
            {
                return languages;
            }

            set
            {
                if (languages != value)
                {
                    languages = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        public List<LanguageLevel> Level
        {
            get
            {
                return levels;
            }

            set
            {
                if (levels != value)
                {
                    levels = value;
                    OnPropertyChanged("Level");
                }
            }
        }

        public List<string> LanguageLevel
        {
            get
            {
                for (int i = 0; i < levels.Count; i++)
                {
                    languagelevel.Add(languages[i] + " " + levels[i].ToString());
                }
                return languagelevel;
            }
            set
            {
                if (languagelevel != null)
                {
                    languagelevel = value;
                }
            }
        }

        // validations
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
                if (columnName == "EmploymentDate")
                {
                    if (employmentDate > DateTime.Now) return "Please enter a valid date. Dates in the future are not allowed.";
                    if (employmentDate == default) return "Birth date is required";
                    else return "";
                }
                return "";
            }
        }

        private readonly string[] _validatedProperties = { "Name", "LastName", "Email", "Password", "PhoneNumber", "BirthDate", "EmploymentDate", "LanguageSkills" };

        // checks if all properties are valid
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

        public string Error => null;

        public TutorDTO()
        {
            languages = new List<string>();
            levels = new List<LanguageLevel>();
            languagelevel = new List<string>();
        }

        public TutorDTO(Tutor tutor)
        {
            this.Id = tutor.Id;
            Name = tutor.Profile.Name;
            LastName = tutor.Profile.LastName;
            Gender = tutor.Profile.Gender;
            BirthDate = tutor.Profile.DateOfBirth;
            PhoneNumber = tutor.Profile.PhoneNumber;
            Email = tutor.Profile.Email;
            Password = tutor.Profile.Password;
            this.role = tutor.Profile.Role;
            this.employmentDate = tutor.EmploymentDate;
            this.languages = tutor.Skill.Language;
            this.levels = tutor.Skill.Level;
            this.languagelevel = new List<string>();
        }

        public Tutor ToTutor()
        {
            return new Tutor(Id, name, lastName, gender, birthDate, phoneNumber, email, password, role, employmentDate, languages, levels);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
