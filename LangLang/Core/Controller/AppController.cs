
namespace LangLang.Core.Controller
{
    public class AppController
    {
        public readonly ExamSlotController ExamSlotController;
        public readonly LoginController LoginController;
        public readonly ExamApplicationController ExamApplicationController;
        public readonly ExamResultController ExamResultController;
        public readonly PenaltyPointController PenaltyPointController;

        public AppController()
        {
            ExamSlotController = new();
            LoginController = new();
            ExamApplicationController = new();
            ExamResultController = new();
            PenaltyPointController = new();
        }


    }
}

