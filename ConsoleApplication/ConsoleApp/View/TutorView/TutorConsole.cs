﻿using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.ConsoleApp.View.TutorView
{
    public class TutorConsole
    {
        public Tutor tutor { get; set; }

        public TutorConsole(Profile tutorProfile)
        {
            TutorService tutorService = new();
            tutor = tutorService.Get(tutorProfile.Id);
            Run();
        }
        public void Run()
        {
            bool loggedIn = true;

            while (loggedIn)
            {
                Console.WriteLine("-----------------------------------------------------------------------");

                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        WorkWithCourses();
                        break;
                    case "2":
                        WorkWithExamSlots();
                        break;
                    case "3":
                        Logout();
                        loggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                Console.WriteLine("-----------------------------------------------------------------------");

            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("Tutor Console Menu");
            Console.WriteLine("1. Work with courses");
            Console.WriteLine("2. Work with exam slots");
            Console.WriteLine("3. Log out");
            Console.Write("Enter your choice: ");
        }

        private void WorkWithCourses()
        {
            Console.WriteLine("Working with courses...");
            CoursesView coursesView = new(tutor);
        }

        private void WorkWithExamSlots()
        {
            Console.WriteLine("Working with exam slots...");
            ExamsView examsView = new(tutor);
        }

        private void Logout()
        {
            Console.WriteLine("Logging out...");
        }
    }
}