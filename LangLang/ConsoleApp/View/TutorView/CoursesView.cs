using LangLang.ConsoleApp.GenericStructures;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.ConsoleApp.View.TutorView
{
    public class CoursesView
    {

        public void DisplayCourses(List<Course> courses)
        {
            var table = new GenericTable<Course>(courses, true);
            table.DisplayTable();

            // You can add more functionality here, like selecting a course
            // Example:
            // var selectedCourse = table.SelectRow();
            // Console.WriteLine($"Selected course: {selectedCourse?.Name}");
        }

    }
}
