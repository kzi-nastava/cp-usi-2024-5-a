using LangLang.Aplication.UseCases;
using LangLang.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Core.Controller
{
    public class AppController
    {
        public readonly TutorController TutorController;
        public readonly CourseController CourseController;
        public readonly DirectorController DirectorController;
        public readonly EnrollmentRequestController EnrollmentRequestController;
        public readonly WithdrawalRequestController WithdrawalRequestController;
        public readonly ExamSlotController ExamSlotController;
        public readonly LoginController LoginController;
        public readonly ExamApplicationController ExamApplicationController;
        public readonly GradeController GradeController;
        public readonly ExamResultController ExamResultController;
        public readonly TutorRatingController TutorRatingController;
        public readonly PenaltyPointController PenaltyPointController;
        public readonly MessageController MessageController;

        public AppController()
        {
            TutorController = new();
            CourseController = new();
            DirectorController = new();
            EnrollmentRequestController = new();
            WithdrawalRequestController = new();
            ExamSlotController = new();
            LoginController = new(TutorController, DirectorController);
            ExamApplicationController = new();
            GradeController = new();
            TutorRatingController = new();
            ExamResultController = new();
            PenaltyPointController = new();
            MessageController = new();
        }

        public Profile? GetProfile(int id, UserType role)
        {
            var studentService = new StudentService();
            return role switch
            {
                UserType.Student => studentService.Get(id)?.Profile,
                UserType.Tutor => TutorController.Get(id)?.Profile,
                UserType.Director => DirectorController.GetAll()?.FirstOrDefault()?.Profile,
                _ => null,
            };
        }

        public bool EmailExists(string email)
        {
            var studentService = new StudentService();

            if (EmailExistsInList(studentService.GetAll(), email)) return true;
            if (EmailExistsInList(TutorController.GetAll(), email)) return true;
            if (EmailExistsInList(DirectorController.GetAll(), email)) return true;
            
            return false;
        }



        public bool EmailExists(string email, int id, UserType role)
        {
            var studentService = new StudentService();

            if (role == UserType.Student)
                return EmailExistsInList(studentService.GetAll(), email, id);
           
            if (role == UserType.Tutor)
                return EmailExistsInList(TutorController.GetAll(), email, id);
            
            return EmailExistsInList(DirectorController.GetAll(), email, id);
        }

        private bool EmailExistsInList<T>(List<T> list, string email) where T : IProfileHolder
        {
            foreach (T item in list)
            {
                if (item.Profile.Email == email) return true;
            }
            return false;
        }

        private bool EmailExistsInList<T>(IEnumerable<T> list, string email, int id) where T : IProfileHolder
        {
            foreach (T item in list)
            {
                if (item.Profile.Email == email && item.Profile.Id != id) return true;
            }
            return false;
        }
    }
}
