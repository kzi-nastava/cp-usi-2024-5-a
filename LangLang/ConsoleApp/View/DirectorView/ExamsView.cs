using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.ConsoleApp.GenericStructures;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.ConsoleApp.View.DirectorView
{
    public class ExamsView
    {
        private List<ExamSlot> exams { get; set; }
        public ExamsView()
        {
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
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateExam();
                        break;
                    case "2":
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
            
            if (!exam.TimeSlot.IsInFuture())
            {
                Console.WriteLine("Exam cannot be created because the date is not in the future.");
                return;
            }

            exam.CreatedAt = DateTime.Now;
            exam.TutorId = SmartSystem.GetTutorForExam(exam);
            exam.Modifiable = true;
            if(exam.TutorId == -1)
            {
                Console.WriteLine("There are no suitable tutors for inserted exam parameters.");
                return;
            }
            ExamSlotService service = new();
            bool added = service.Add(exam);

            if (!added) {
                Console.WriteLine("Choose another exam date or time.");
                return; 
            }

            Console.WriteLine("Exam created successfully.");
            ReloadExams();
        }

        private void ReloadExams()
        {
            ExamSlotService service = new();
            exams = service.GetAll();
        }
    }
}
