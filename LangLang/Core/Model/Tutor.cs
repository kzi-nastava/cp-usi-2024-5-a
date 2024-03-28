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
        private Profile Profile { get; set; }

        // Constructor
        public Tutor()
        {
            Profile = new Profile();
        }
        public Tutor(Profile profile)
        {
            Profile = profile;
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
