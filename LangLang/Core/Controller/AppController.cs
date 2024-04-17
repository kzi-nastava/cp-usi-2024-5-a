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
        private TutorController _tutorController;
        private CourseController _courseController;
        private StudentController _studentController;
        private EnrollmentRequestController _enrollmentRequestController;
        private ExamSlotController _examSlotController;
        private LoginController _loginController;
        public AppController()
        {
            this._tutorController = new TutorController();
            this._courseController = new CourseController();
            this._studentController = new StudentController();
            this._enrollmentRequestController = new EnrollmentRequestController();
            this._examSlotController = new ExamSlotController();
            this._loginController = new LoginController(_studentController, _tutorController);
        }

        public LoginController LoginController
        {
            get { return this._loginController; }
        }

        public TutorController TutorController
        {
            get { return this._tutorController; }
            set { this._tutorController = value; }
        }

        public CourseController CourseController
        {
            get { return this._courseController; }
            set { this.CourseController = value; }
        }

        public StudentController StudentController
        {
            get { return this._studentController; }
            set { this._studentController = value; }
        }

        public EnrollmentRequestController EnrollmentRequestController
        {
            get { return this._enrollmentRequestController; }
            set { this._enrollmentRequestController = value; }
        }

        public ExamSlotController ExamSlotController
        {
            get { return this._examSlotController; }
            set { this._examSlotController = value; }
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
