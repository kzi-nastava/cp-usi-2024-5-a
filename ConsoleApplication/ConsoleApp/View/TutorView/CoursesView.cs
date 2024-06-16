
﻿using LangLang.BusinessLogic.UseCases;
using LangLang.Configuration;
using LangLang.Domain.Models;
using ConsoleApplication.ConsoleApp.GenericStructures;

namespace ConsoleApplication.ConsoleApp.View.TutorView
{
    public class CoursesView
    {
        private List<Course> courses { get; set; }
        private Tutor tutor { get; set; }
        public CoursesView(Tutor loggedIn)
        {
            tutor = loggedIn;
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
                Console.WriteLine("2. Update Course");
                Console.WriteLine("3. Delete Course");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateCourse();
                        break;
                    case "2":
                        UpdateCourse();
                        break;
                    case "3":
                        DeleteCourse();
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
        public void CreateCourse()
        {
            Console.WriteLine("Creating new course...");
            Course course = GenericForm.CreateEntity<Course>();

            // make sure that days are unique and sorted
            course.Days = course.Days.Distinct().ToList();
            var daysArray = course.Days.ToArray();
            Array.Sort(daysArray);
            course.Days = daysArray.ToList();

            course.CreatedAt = DateTime.Now;
            course.TutorId = tutor.Profile.Id;
            course.Modifiable = true;

            if (!IsValid(course))
            {
                Console.WriteLine("Course can not be created. Tutor doesn't know given language on that level or the start date is not in the future.");
                return;
            }

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
        private bool IsValid(Course course)
        {
            TutorService tutorService = new();
            Tutor tutor = tutorService.Get(course.TutorId);
            if (tutor.HasLanguageLevel(course.Language, course.Level) && DateTime.Now < course.StartDateTime) return true;
            return false;
        }
        public void UpdateCourse()
        {
            Console.WriteLine("Updating course...");
            var table = new GenericTable<Course>(courses, true);
            Course selected = table.SelectRow();

            if (selected == null) return;

            if (!selected.CanChange())
            {
                Console.WriteLine($"Can't update course, there is less than {Constants.COURSE_MODIFY_PERIOD} days before course or it has already started.");
                return;
            }
            CourseService service = new();
            Course updated = selected;
            Console.WriteLine("Updating course details:");
            updated = GenericForm.UpdateEntity<Course>(selected);

            if (!IsValid(updated))
            {
                Console.WriteLine("Course can not be updated. Tutor doesn't know given language on that level or the start date is not in the future.");
                return;
            }

            updated.GenerateTimeSlots();
            if (!service.CanCreateOrUpdate(updated))
            {
                Console.Write($"Course can not be created. Try again with another date and class time.");
                return;
            }
            service.Update(updated);
            Console.WriteLine("Course updated successfully.");
        }
        public void DeleteCourse()
        {
            Console.WriteLine("Deleting exam...");
            var table = new GenericTable<Course>(courses, true);
            Course selected = table.SelectRow();
            if (selected == null) return;

            bool confirmed = ConfirmationMessage();
            if (confirmed)
            {
                if (!selected.CanChange())
                {
                    Console.Write($"Can't delete course, there is less than {Constants.COURSE_CANCELLATION_PERIOD} days before course.");
                }
                else
                {
                    CourseService service = new();
                    service.Delete(selected.Id);
                    Console.Write("Course successfully deleted.");
                }
            }

        }
        public void DisplayCourses()
        {
            var table = new GenericTable<Course>(courses, true);
            table.DisplayTable();
        }

        private void ReloadCourses()
        {
            CourseService service = new();
            courses = service.GetByTutor(tutor.Id);
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