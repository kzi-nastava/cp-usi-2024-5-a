using LangLang.Core.Repository.Serialization;
using LangLang.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using LangLang.Core.Controller;

namespace LangLang.Core.Model
{
    public class Tutor : ISerializable, IProfileHolder
    {
        private Skill _skill;

        private DateTime _employmentDate;

        private Profile _profile;

        public Skill Skill
        {
            get { return _skill; }
            set { _skill = value; }
        }

        public DateTime EmploymentDate
        {
            get { return _employmentDate; }
            set { _employmentDate = value; }
        }
        public Profile Profile
        {
            get { return _profile; }
            private set { _profile = value; }
        }

        public Tutor(int id, string name, string lastName, UserGender gender, DateTime dateOfBirth, string phoneNumber, string email, string password, UserType role, DateTime employmentDate, List<string> languages, List<LanguageLevel>levels) {
            _profile = new Profile(id, name, lastName, gender, dateOfBirth, phoneNumber, email, password, role);
            _employmentDate = employmentDate;
            _skill = new(languages, levels);
            }

        public Tutor()
        {
            _skill = new();
            _employmentDate = DateTime.MinValue;
            _profile = new();
        }

        public int Id
        {
            get { return Profile.Id; }
        }

        public void FromCSV(string[] values)
        {
            _profile = new Profile(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8]);

            if (!DateTime.TryParseExact(values[9], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _employmentDate))
            {
                throw new FormatException("Date is not in the correct format.");
            }

            for (int i = 10; i < values.Length; i++)
            {
                string[] languageSkill = values[i].Split(',');

                if (languageSkill.Length != 2)
                {
                    throw new FormatException("Language and skill pair is not in the correct format.");
                }
                LanguageLevel level;
                if (Enum.TryParse(languageSkill[1], out level))
                {
                    _skill.Language.Add(languageSkill[0]);
                    _skill.Level.Add(level);
                }
            }
        }

        public string[] ToCSV()
        {
            if (_skill.Language.Count > 0)
            {
                return new string[] {
            _profile.ToString(),
            _employmentDate.ToString("yyyy-MM-dd"),
            ListsToCSV(_skill) };
            }
            else
            {
                return new string[] {
            _profile.ToString(),
            _employmentDate.ToString("yyyy-MM-dd") };
            }

        }

        public string ListsToCSV(Skill skill)
        {
            StringBuilder sb = new StringBuilder();
            var length = skill.Language.Count();

            while (length > 0)
            {
                sb.Append(skill.Language[length - 1]).Append(",").Append(skill.Level[length - 1]);
                length--;
                if (length > 0)
                {
                    sb.Append("|");
                }
            }

            return sb.ToString();
        }

    }
}
 