using LangLang.Core.Repository.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangLang.Core.Model
{
    public class Tutor : ISerializable, IProfileHolder
    {

        public Profile Profile { get; set; }

        public Skill Skill{ get; set; }

        public DateTime EmploymentDate{ get; set; }

        public int Id => Profile.Id;

        public Tutor(int id, string name, string lastName, Gender gender, DateTime birthDate, string phoneNumber, string email, string password, UserType role, bool isDeleted, DateTime employmentDate, List<string> languages, List<LanguageLevel>levels) {
            Profile = new (id, name, lastName, gender, birthDate, phoneNumber, email, password, role, isDeleted);
            EmploymentDate = employmentDate;
            Skill = new(languages, levels); 
        }

        public Tutor()
        {
            Profile = new();
            Skill = new();
        }

        public void FromCSV(string[] values)
        {
            Profile = new Profile(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9]);

            try {
                EmploymentDate = DateTime.ParseExact(values[10], Constants.DATE_FORMAT, null);
            } catch 
            {
                throw new FormatException("Date is not in the correct format.");
            }

            for (int i = 11; i < values.Length; i++)
            {
                string[] languageSkill = values[i].Split(',');

                if (languageSkill.Length != 2)
                {
                    throw new FormatException("Language and skill pair is not in the correct format.");
                }

                try
                {
                    LanguageLevel level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), languageSkill[1]);
                    Skill.Language.Add(languageSkill[0]);
                    Skill.Level.Add(level);
                }
                catch (ArgumentException)
                {
                    throw new FormatException("Invalid skill level.");
                }

            }
        }

        public string[] ToCSV()
        {
            List<string> csvValues = new List<string>();

            csvValues.Add(Profile.ToString());
            csvValues.Add(EmploymentDate.ToString(Constants.DATE_FORMAT));

            if (Skill.Language.Count > 0)
            {
                csvValues.Add(ListToCSV(Skill));
            }

            return csvValues.ToArray();
        }

        public string ListToCSV(Skill skill)
        {
            List<string> languageLevels = new List<string>();

            for (int i = 0; i < skill.Language.Count; ++i)
            {
                string languageLevel = $"{skill.Language[i]},{skill.Level[i]}";
                languageLevels.Add(languageLevel);
            }

            return string.Join("|", languageLevels);
        }

    }
}
 