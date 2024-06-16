using LangLang.BusinessLogic.UseCases;
using ConsoleApplication.ConsoleApp.View.DirectorView;
using ConsoleApplication.ConsoleApp.View.TutorView;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.ConsoleApp.View
{
    public class LogIn
    {
        public LogIn()
        {
            bool isAuthenticated = false;

            while (!isAuthenticated)
            {
                Console.Clear();

                Console.WriteLine("Login Form");
                Console.WriteLine("----------");

                Console.Write("Username: ");
                string username = Console.ReadLine();

                Console.Write("Password: ");
                string password = ReadPassword();

                Profile profile = TrySignUp(username, password);
                if (profile != null)
                {
                    Console.WriteLine("\nLogin successful!");
                    isAuthenticated = true;
                    OpenAppropriateConsole(profile);
                }
                else
                {
                    Console.WriteLine("\nIncorrect username or password. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        private static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return password;
        }
        private static Profile? TrySignUp(string email, string password)
        {
            try
            {
                var loginService = new LoginService();
                Profile profile = loginService.GetProfileByCredentials(email, password);
                return profile;
            }
            catch (AuthenticationException ex)
            {
                return null;
            }
        }
        private void OpenAppropriateConsole(Profile profile)
        {
            if (profile.Role == UserType.Tutor)
            {
                TutorConsole tutorConsole = new(profile);
            }
            else if (profile.Role == UserType.Director)
            {
                DirectorConsole directorConsole = new(profile);
            }
        }
    }

}
