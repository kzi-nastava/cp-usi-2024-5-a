using LangLang.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Controller
{
    public class AppController
    {
        public TutorController TutorController { get; }
        public CourseController CourseController { get; }
        public StudentController StudentController { get; }
        public EnrollmentRequestController EnrollmentRequestController { get; }
        public ExamSlotController ExamSlotController { get; }
        public LoginController LoginController { get; }
        public AppController()
        {
            this.TutorController = new TutorController();
            this.CourseController = new CourseController();
            this.StudentController = new StudentController();
            this.EnrollmentRequestController = new EnrollmentRequestController();
            this.ExamSlotController = new ExamSlotController();
            this.LoginController = new LoginController(StudentController, TutorController);
        }


        public bool EmailExists(string email)
        {

            foreach (Student student in StudentController.GetAllStudents().Values)
            {
                if (student.Profile.Email == email) return true;
            }

            foreach (Tutor tutor in TutorController.GetAllTutors().Values)
            {
                if (tutor.Profile.Email == email) return true;
            }

            // TODO: check if it is the same as directors
            
            return false;
        }

        public bool EmailExists(string email, int id, UserType role)
        {
            if (role == UserType.Student)
            {
                foreach (Student student in StudentController.GetAllStudents().Values)
                {
                    if ((student.Profile.Email == email) && (student.Profile.Id != id)) return true;
                }
            }
            else
            {
                foreach (Tutor tutor in TutorController.GetAllTutors().Values)
                {
                    if ((tutor.Profile.Email == email) && (tutor.Profile.Id != id)) return true;
                }
            }

            // TODO: check if it is the same as directors
            return false;
        }
    }
}
