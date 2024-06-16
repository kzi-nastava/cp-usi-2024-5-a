using LangLang.BusinessLogic.UseCases;
using LangLang.Domain;
using ConsoleApplication.ConsoleApp.GenericStructures;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.ConsoleApp.View.DirectorView
{
    public class TutorsView
    {
        private List<Tutor> tutors { get; set; }
        public TutorsView()
        {
            ReloadTutors();
            Run();
        }
        public void Run()
        {
            bool inExams = true;
            while (inExams)
            {
                Console.WriteLine("-----------------------------------------------------------------------");
                ReloadTutors();
                DisplayTutors();

                Console.WriteLine("Tutor Menu:");
                Console.WriteLine("1. Add Tutor");
                Console.WriteLine("2. Update Tutor");
                Console.WriteLine("3. Delete Tutor");
                Console.WriteLine("4. Show skills");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTutor();
                        break;
                    case "2":
                        UpdateTutor();
                        break;
                    case "3":
                        DeleteTutor();
                        break;
                    case "4":
                        ShowSkills();
                        break;
                    case "5":
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
        public void DisplayTutors()
        {
            var table = new GenericTable<Tutor>(tutors, true);
            table.DisplayTable();
        }
        public void ShowSkills()
        {
            var table = new GenericTable<Tutor>(tutors, true);
            Tutor selected = table.SelectRow();
            if (selected == null) return;
            //var tableSkills = new GenericTable<LanguageLevel>(selected.Skill, true);
            //tableSkills.DisplayTable();
        }
        public void AddTutor()
        {
            Console.WriteLine("Creating new tutor...");
            Tutor tutor = GenericForm.CreateEntity<Tutor>();

            if (!IsValid(tutor))
            {
                Console.WriteLine("Tutor cannot be created because their birthdate is not in the past.");
                return;
            }
            tutor.EmploymentDate = DateTime.Now;
            tutor.Profile.Role = LangLang.Domain.Enums.UserType.Tutor;

            TutorService service = new();
            service.Add(tutor);

            Console.WriteLine("Tutor added successfully.");
            ReloadTutors();
        }
        private bool IsValid(Tutor tutor)
        {
            if(tutor.Profile.BirthDate >= DateTime.Now)
            {
                return false;
            }
            return true;
        }
        public void UpdateTutor()
        {
            Console.WriteLine("Updating tutor...");

            var table = new GenericTable<Tutor>(tutors, true);
            Tutor selected = table.SelectRow();
            if (selected == null) return;

            Tutor updated = selected;
            Console.WriteLine("Updating tutor details:");
            updated = GenericForm.UpdateEntity<Tutor>(selected);

            if (!IsValid(updated))
            {
                Console.WriteLine("Tutor cannot be updated because their birthdate is not in the past or the number of languages and respective levels do not match.");
                return;
            }

            TutorService service = new();
            service.Update(updated);
            Console.WriteLine("Tutor updated successfully.");
        }

        public void DeleteTutor()
        {
            Console.WriteLine("Deleting tutor...");
            var table = new GenericTable<Tutor>(tutors, true);
            Tutor selected = table.SelectRow();
            if (selected == null) return;

            bool confirmed = ConfirmationMessage();
            if (confirmed)
            {
                var courseService = new CourseService();
                var tutorService = new TutorService();
                var examService = new ExamSlotService();

                courseService.DeleteByTutor(selected);
                examService.DeleteByTutor(selected);
                tutorService.Deactivate(selected.Id);
            }
            Console.WriteLine("Tutor has been successfully deleted");
        }

        private void ReloadTutors()
        {
            TutorService service = new();
            tutors = service.GetAll();
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
