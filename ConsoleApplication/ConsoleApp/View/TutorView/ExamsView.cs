using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using ConsoleApplication.ConsoleApp.GenericStructures;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleApplication.ConsoleApp.View.TutorView
{
    public class ExamsView
    {
        private List<ExamSlot> exams { get; set; }
        private Tutor tutor {  get; set; }
        public ExamsView(Tutor loggedIn)
        {
            tutor = loggedIn;
            ReloadExams();
            Run();
        }
        public void Run()
        {
            bool inExams = true;
            while (inExams)
            {
                Console.WriteLine("-----------------------------------------------------------------------");
                ReloadExams();
                DisplayExams();

                Console.WriteLine("Exam Menu:");
                Console.WriteLine("1. Create Exam");
                Console.WriteLine("2. Update Exam");
                Console.WriteLine("3. Delete Exam");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateExam();
                        break;
                    case "2":
                        UpdateExam();
                        break;
                    case "3":
                        DeleteExam();
                        break;
                    case "4":
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
        public void DisplayExams()
        {
            var table = new GenericTable<ExamSlot>(exams, true);
            table.DisplayTable();
        }

        public void CreateExam()
        {
            Console.WriteLine("Creating new exam...");
            ExamSlot exam = GenericForm.CreateEntity<ExamSlot>();
            
            
            exam.CreatedAt = DateTime.Now;
            exam.TutorId = tutor.Profile.Id;
            exam.Modifiable = true;

            if (!IsValid(exam)) { 
                Console.WriteLine("Exam slot can not be created. Tutor doesn't know given language on that level.");
                return;
            }

            ExamSlotService service = new();
            try
            {
                service.Add(exam);
                Console.WriteLine("Exam created successfully.");
                ReloadExams();
            }
            catch
            {
                Console.WriteLine("Choose another exam date or time.");
                return;
            }
            
        }
        private bool IsValid(ExamSlot exam)
        {
            LanguageLevelService languageLevelService = new();
            LanguageLevel ll = languageLevelService.Get(exam.LanguageId);
            if (tutor.HasLanguageLevel(ll.Language, ll.Level)) return true;
            return false;
        }
        public void UpdateExam()
        {
            Console.WriteLine("Updating exam...");

            var table = new GenericTable<ExamSlot>(exams, true);
            ExamSlot selected = table.SelectRow();
            if (selected == null) return;

            ExamSlotService service = new();
            if (!service.CanBeUpdated(selected))
            {
                Console.WriteLine($"Can't update exam, there is less than {Constants.EXAM_MODIFY_PERIOD} days before exam or exam has passed.");
                return;
            }

            ExamSlot updated = selected;
            Console.WriteLine("Updating exam details:");
            updated = GenericForm.UpdateEntity<ExamSlot>(selected);
            if (!service.CanCreateExam(updated))
            {
                Console.Write($"Exam can not be updated. You must choose another exams date or time.");
                return;
            }
            service.Update(updated);
            Console.WriteLine("Exam updated successfully.");
        }

        public void DeleteExam()
        {
            Console.WriteLine("Deleting exam...");
            var table = new GenericTable<ExamSlot>(exams, true);
            ExamSlot selected = table.SelectRow();
            if (selected == null) return;

            bool confirmed = ConfirmationMessage();
            if (confirmed)
            {
                ExamSlotService service = new();
                try {
                    service.Delete(selected.Id);
                    Console.Write("Exam successfully deleted.");

                }
                catch
                {
                    Console.Write($"Can't delete exam, there is less than {Constants.EXAM_MODIFY_PERIOD} days before exam.");

                }
                
            }

        }

        private void ReloadExams()
        {
            ExamSlotService service = new();
            exams = service.GetByTutor(tutor);
        }
        private bool ConfirmationMessage()
        {
            Console.WriteLine("Are you sure?");
            
            while (true)
            {
                Console.Write("Enter your choice (Y/N): ");
                string choice = Console.ReadLine().Trim().ToUpper();
                switch (choice)
                {
                    case "Y":
                        return true;
                    case "N":
                        return false;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
            
        }
    }
}
