using LangLang.Core.Repository.Serialization;
using LangLang.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LangLang.Core.Model
{
    public class Tutor : ISerializable
    {
        private Profile _profile { get; set; }
        private Dictionary<string, LanguageLevel> _languageSkills { get; set; }
        private DateTime _employmentDate { get; set; }

        public Tutor(int id, string name, string lastName, UserGender gender, DateTime dateOfBirth, string phoneNumber, string email, string password, UserType role, DateTime employmentDate, Dictionary<string, LanguageLevel> languageSkills) {
            _profile = new Profile(id, name, lastName, gender, dateOfBirth, phoneNumber, email, password, role);
            _employmentDate = employmentDate;
            _languageSkills = languageSkills;
            }

        public void FromCSV(string[] values)
        {
            _profile = new Profile(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8]);
            DateTime employmentDate;

            if (!DateTime.TryParseExact(values[9], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out employmentDate))
            {
                throw new FormatException("Date is not in the correct format.");
            }
            _employmentDate = employmentDate;

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
                    _languageSkills.Add(languageSkill[0], level);
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                _profile.ToString(),
                _employmentDate.ToString("yyyy-MM-dd"),
                DictToCSV(_languageSkills)
            };

            return csvValues;
        }

        public string DictToCSV(Dictionary<string, LanguageLevel> skills){
            StringBuilder sb = new StringBuilder();
            var length = skills.Count();
            foreach (var skill in skills){
                sb.Append(skill.Key).Append(",").Append(skill.Value);
                if (--length>0) sb.Append("|");
            }
            return sb.ToString();
        }

        /*
        public List<Course> GetCourses(ref Dictionary<int, Course> courses)
        {
        }
        */
        //GetExamSlots takes hashmap of all examslots and returns list of examslots that this tutor created
        /*
        public List<ExamSlot> GetExamSlots(ref Dictionary<int, ExamSlot> examSlots)
        {
            List<ExamSlot> examSlotList = new List<ExamSlot>();

            foreach (KeyValuePair<int, ExamSlot> pair in examSlots)
            {
                if (pair.Value.Course.Tutor.Profile.Email == Profile.Email)
                {
                    examSlotList.Add(pair.Value);
                }
            }

            return examSlotList;
        }
        */
        //add searchCourses and searchExamSlots
    }
}
 