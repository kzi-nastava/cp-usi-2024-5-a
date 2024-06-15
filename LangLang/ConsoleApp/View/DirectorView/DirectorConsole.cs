using LangLang.BusinessLogic.UseCases;
using LangLang.ConsoleApp.View.DirectorView;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.ConsoleApp.View.DirectorView
{
    public class DirectorConsole
    {
        Director director { get; set; }

        public DirectorConsole(Profile directorProfile)
        {
            DirectorService directorService = new();
            director = directorService.Get(directorProfile.Id);
            Run();
        }
        public void Run()
        {
            bool loggedIn = true;

            while (loggedIn)
            {
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
                        WorkWithTutors();
                        break;
                    case "4":
                        Logout();
                        loggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("Director Console Menu");
            Console.WriteLine("1. Work with courses");
            Console.WriteLine("2. Work with exam slots");
            Console.WriteLine("3. Work with tutors");
            Console.WriteLine("4. Log out");
            Console.Write("Enter your choice: ");
        }

        private void WorkWithCourses()
        {
            Console.WriteLine("Working with courses...");
            CoursesView coursesView = new(director);
        }

        private void WorkWithExamSlots()
        {
            Console.WriteLine("Working with exam slots...");
        }
        private void WorkWithTutors()
        {
            Console.WriteLine("Working with tutors...");
            // Add logic to work with tutors
        }
        private void Logout()
        {
            Console.WriteLine("Logging out...");
            // Add logic to perform logout actions
        }
    }
}
