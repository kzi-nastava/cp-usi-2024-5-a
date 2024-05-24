﻿
namespace LangLang.Core.Controller
{
    public class AppController
    {
        public readonly CourseController CourseController;
        public readonly ExamSlotController ExamSlotController;
        public readonly ExamApplicationController ExamApplicationController;
        public readonly GradeController GradeController;
        public readonly ExamResultController ExamResultController;
        public readonly PenaltyPointController PenaltyPointController;

        public AppController()
        {
            CourseController = new();
            ExamSlotController = new();
            ExamApplicationController = new();
            GradeController = new();
            ExamResultController = new();
            PenaltyPointController = new();
        }


    }
}

