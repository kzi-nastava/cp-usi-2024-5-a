using LangLang.Core.Repository.Serialization;
using System;
using System.Text;

namespace LangLang.Core.Model
{
    class Student : ISerializable
    {

        private Profile _profile;
        private string _professionalQualification;
        private bool _canModifyInfo; // NOTE: This attribute may not be necessary.

        public Profile Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }

        public string ProfessionalQualification
        {
            get { return _professionalQualification; }
            set { _professionalQualification = value; }
        }

        public bool CanModifyInfo
        {
            get { return _canModifyInfo; }
            set { _canModifyInfo = value; }
        }
        public Student() { }

        public Student(string name, string lastName, UserGender gender, DateTime dateOfBirth, string phoneNumber, string email, string password, UserType role, string professionalQualification)
        {
            Profile = new Profile(name, lastName, gender, dateOfBirth, phoneNumber, email, password, role);
            ProfessionalQualification = professionalQualification;
            CanModifyInfo = true;
        }

        public void FromCSV(string[] values) // TODO: consider moving a portion of code into the Profile class
        {
            if (values.Length < 9)
            {
                throw new ArgumentException("Insufficient number of values provided.");
            }

            if (!Enum.TryParse(values[2], out UserGender gender)
                || !Enum.TryParse(values[7], out UserType role)
                || !DateTime.TryParseExact(values[3], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dateOfBirth))
            {
                throw new FormatException("One or more tokens are not in the correct format.");
            }

            Profile = new Profile(values[0], values[1], gender, dateOfBirth, values[4], values[5], values[6], role);

            ProfessionalQualification = values[8];

            if (!bool.TryParse(values[9], out bool canModifyInfo))
            {
                throw new FormatException("CanModifyInfo token is not in the correct format.");
            }

            CanModifyInfo = canModifyInfo;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Profile.ToString(),
                ProfessionalQualification,
                CanModifyInfo.ToString()
            };

            return csvValues;
        }
    }
}
