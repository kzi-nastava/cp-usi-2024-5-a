
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


    }
}

