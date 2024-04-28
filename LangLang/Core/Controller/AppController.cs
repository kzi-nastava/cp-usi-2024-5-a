using LangLang.Core.Model;

namespace LangLang.Core.Controller
{
    public class AppController
    {
        public readonly TutorController TutorController;
        public readonly CourseController CourseController;
        public readonly StudentController StudentController;
        public readonly EnrollmentRequestController EnrollmentRequestController;
        public readonly ExamSlotController ExamSlotController;
        public readonly LoginController LoginController;
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
