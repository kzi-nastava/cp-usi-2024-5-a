using ConsoleApplication.ConsoleApp.View.DirectorView;
using ConsoleApplication.ConsoleApp.View.TutorView;
using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System.Security.Authentication;


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
                PrintSeparatorLine();

                Console.Write("Username: ");
                string username = Console.ReadLine();

                Console.Write("Password: ");
                string password = ReadPassword();

                Profile profile = TrySignUp(username, password);
                if (profile != null)
                {
                    Console.WriteLine("\nLogin successful!");
                    isAuthenticated = true;
                    IConsoleView console = ConsoleViewFactory.GetConsoleView(profile);
                    console.DisplayOutput(profile);
                }
                else
                {
                    Console.WriteLine("\nIncorrect username or password. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        public static void PrintSeparatorLine()
        {
            int width = Console.WindowWidth;
            string separator = new string('-', 210);
            Console.WriteLine(separator);
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
    }

}
