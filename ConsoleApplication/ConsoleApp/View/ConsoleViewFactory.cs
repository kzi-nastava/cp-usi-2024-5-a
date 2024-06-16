using ConsoleApplication.ConsoleApp.View.DirectorView;
using ConsoleApplication.ConsoleApp.View.TutorView;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.ConsoleApp.View
{
    public static class ConsoleViewFactory
    {
        public static IConsoleView GetConsoleView(Profile user)
        {
            return user.Role switch
            {
                UserType.Director => new DirectorConsole(),
                UserType.Tutor => new TutorConsole(),
                _ => throw new ArgumentException("Invalid user role"),
            };
        }
    }
}
