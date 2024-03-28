using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LangLang.Core.Model;
namespace LangLang.Core.Model
{
    class Tutor
    {
        // Profile attribute of type Profile
        private Profile _profile { get; set; }
        private Dictionary<string, LanguageLevel> _languageSkills { get; set; }

        // Constructor
        public Tutor()
        {
            // Initialize empty profile and language skills
            _profile = new Profile();
            _profile.Role = UserType.Tutor;
            _languageSkills = new Dictionary<string, LanguageLevel>();
        }
        public Tutor(Profile profile, Dictionary<string, LanguageLevel> languageSkills)
        {
            _profile = profile;
            _languageSkills = languageSkills;
        }

        //Getters and setters
        public Profile Profile
        {
            get { return _profile; }
            private set { _profile = value; }
        }
        public Dictionary<string, LanguageLevel> LanguageSkills
        {
            get { return _languageSkills; }
            private set { _languageSkills = value; }
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
