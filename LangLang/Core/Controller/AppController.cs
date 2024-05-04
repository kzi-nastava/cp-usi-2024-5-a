using LangLang.Core.Model;

namespace LangLang.Core.Controller
{
    public class AppController
    {
        public readonly TutorController TutorController;
        public readonly CourseController CourseController;
        public readonly StudentController StudentController;
        public readonly EnrollmentRequestController EnrollmentRequestController;
        public readonly WithdrawalRequestController WithdrawalRequestController;
        public readonly ExamSlotController ExamSlotController;
        public readonly LoginController LoginController;
        public readonly ExamAppRequestController ExamAppRequestController;
        public readonly GradeController GradeController;
        public readonly ExamResultController ExamResultController;
      
        public AppController()
        {
            TutorController = new();
            CourseController = new();
            StudentController = new();
            EnrollmentRequestController = new();
            WithdrawalRequestController = new();
            ExamSlotController = new();
            LoginController = new(StudentController, TutorController);
            ExamAppRequestController = new();
            GradeController = new();
            ExamResultController = new();
        }


        public bool EmailExists(string email)
        {

            foreach (Student student in StudentController.GetAllStudents())
            {
                if (student.Profile.Email == email) return true;
            }

            foreach (Tutor tutor in TutorController.GetAllTutors())
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
                foreach (Student student in StudentController.GetAllStudents())
                {
                    if ((student.Profile.Email == email) && (student.Profile.Id != id)) return true;
                }
            }
            else
            {
                foreach (Tutor tutor in TutorController.GetAllTutors())
                {
                    if ((tutor.Profile.Email == email) && (tutor.Profile.Id != id)) return true;
                }
            }

            // TODO: check if it is the same as directors
            return false;
        }
    }
}
