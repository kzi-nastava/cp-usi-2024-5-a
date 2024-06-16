
using LangLang.ConsoleApp.GenericStructures;
using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleApp1.ConsoleApp.View.DirectorView
{
    public class CoursesView
    {
        private List<Course> courses { get; set; }
        private Director director { get; set; }
        public CoursesView(Director loggedIn)
        {
            director = loggedIn;
            ReloadCourses();
            Run();
        }
        public void Run()
        {
            bool inCourses = true;
            while (inCourses)
            {
                Console.WriteLine("-----------------------------------------------------------------------");
                ReloadCourses();
                DisplayCourses();

                Console.WriteLine("Course Menu:");
                Console.WriteLine("1. Create Course");
                Console.WriteLine("2. Assign tutor");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateCourse();
                        break;
                    case "2":
                        AssignTutor();
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------------------------");

            }
        }
        public void CreateCourse()
        {
            Console.WriteLine("Creating new course...");
            Course course = GenericForm.CreateEntity<Course>();
            course.CreatedAt = DateTime.Now;
            course.TutorId = Constants.DELETED_TUTOR_ID;
            course.Modifiable = true;
            course.GenerateTimeSlots();
            CourseService service = new();

            if (!service.CanCreateOrUpdate(course))
            {
                Console.WriteLine("Course can not be created. Try again with another date and class time.");
                return;
            }
            service.Add(course);
            Console.WriteLine("Course created successfully.");
            ReloadCourses();
        }

        public void AssignTutor()
        {
            Console.WriteLine("Assigning tutor...");
            CourseService service = new();
            List<Course> show = service.GetWithoutTutor();
            var table = new GenericTable<Course>(show, true);
            table.DisplayTable();
            Course selected = table.SelectRow();
            if (selected == null) return;

            selected.TutorId = SmartSystem.MostSuitableTutor(selected);
            if (selected.TutorId == -1)
            {
                Console.WriteLine("There is no suitable tutor for this course at the moment.");
                return;
            }
            service.Update(selected);
            Console.WriteLine("Tutor is successfuly assigned.");
            ReloadCourses();


        }
        public void DisplayCourses()
        {
            var table = new GenericTable<Course>(courses, true);
            table.DisplayTable();
        }

        private void ReloadCourses()
        {
            CourseService service = new();
            courses = service.GetAll();
        }

        private bool ConfirmationMessage()
        {
            Console.WriteLine("Are you sure?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            Console.Write("Enter your choice (1/2): ");
            string choice = Console.ReadLine();
            while (true)
            {
                switch (choice)
                {
                    case "1":
                        return true;
                    case "2":
                        return false;
                    default:
                        Console.WriteLine("Invalid choice try again.");
                        break;
                }
            }

        }

    }
}
