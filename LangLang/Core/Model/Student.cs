using LangLang.Core.Repository.Serialization;
using System;

namespace LangLang.Core.Model
{
    public class Student : ISerializable
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

        public Student(int id, string name, string lastName, UserGender gender, DateTime dateOfBirth, string phoneNumber, string email, string password, UserType role, string professionalQualification)
        {
            Profile = new Profile(id, name, lastName, gender, dateOfBirth, phoneNumber, email, password, role);
            ProfessionalQualification = professionalQualification;
            CanModifyInfo = true;
        }

        public void FromCSV(string[] values) 
        {
            Profile = new Profile(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8]);

            ProfessionalQualification = values[9];

            if (!bool.TryParse(values[10], out bool canModifyInfo))
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
