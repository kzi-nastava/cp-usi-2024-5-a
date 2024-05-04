﻿using LangLang.Core.Model;
using LangLang.Core.Repository;
using System.Collections.Generic;

namespace LangLang.Core.Controller
{
    public class AppController
    {
        public readonly TutorController TutorController;
        public readonly CourseController CourseController;
        public readonly StudentController StudentController;
        public readonly DirectorController DirectorController;
        public readonly EnrollmentRequestController EnrollmentRequestController;
        public readonly WithdrawalRequestController WithdrawalRequestController;
        public readonly ExamSlotController ExamSlotController;
        public readonly LoginController LoginController;
        public readonly ExamAppRequestController ExamAppRequestController;
        public readonly GradeController GradeController;
        public readonly TutorRatingController TutorRatingController;
    
        public AppController()
        {
            TutorController = new();
            CourseController = new();
            StudentController = new();
            DirectorController = new();
            EnrollmentRequestController = new();
            WithdrawalRequestController = new();
            ExamSlotController = new();
            LoginController = new(StudentController, TutorController, DirectorController);
            ExamAppRequestController = new();
            GradeController = new();
            TutorRatingController = new();
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

            foreach (Director director in DirectorController.GetAllDirectors())
            {
                if (director.Profile.Email == email) return true;
            }
            
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
            else if (role == UserType.Tutor)
            {
                foreach (Tutor tutor in TutorController.GetAllTutors())
                {
                    if ((tutor.Profile.Email == email) && (tutor.Profile.Id != id)) return true;
                }
            }
            else
            {
                foreach (Director director in DirectorController.GetAllDirectors())
                {
                    if ((director.Profile.Email == email) && (director.Profile.Id != id)) return true;
                }
            }

            return false;
        }
    }
}
