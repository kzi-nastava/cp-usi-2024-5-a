using System;
using LangLang.Core.Repository.Serialization;

namespace LangLang.Core.Model
{
    class Director : ISerializable
    {

        private Profile _profile;

        public Profile Profile
        {
            get {return _profile;}
            set { _profile = value;}
        }

        public Director(string name, string lastName, UserGender gender, DateTime dateOfBirth, string phoneNumber, string email, string password, UserType role) {
            _profile = new Profile(name, lastName, gender, dateOfBirth, phoneNumber, email, password, role);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { _profile.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            _profile = new Profile(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]); // TODO: when implemented in Profile, consider how to catch an exception if necessary
       
        }
    }
}
