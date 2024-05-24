using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;

namespace LangLang.BusinessLogic.UseCases
{

    public class LoginService
    {
        public LoginService() { }

        public Profile GetProfileByCredentials(string email, string password)
        {
            var studentService = new StudentService();
            var tutorService = new TutorService();
            var directorService = new DirectorService();
            var profile = (GetProfile(studentService.GetAll(), email, password)
                            ?? GetProfile(tutorService.GetAll(), email, password))
                            ?? GetProfile(directorService.GetAll(), email, password)
                            ?? throw new AuthenticationException("Invalid email address.");
            return profile; // profile with the given credentials exists

        }

        private Profile? GetProfile<userType>(List<userType> users, string email, string password) where userType : IProfileHolder
        {
            userType user = users.FirstOrDefault(user => user.Profile.Email == email);

            if (user == null || user.Profile == null)
            {
                return null; // It might be in another container, so we don't throw an exception here.
            }

            if (user.Profile.Password != password)
            {
                throw new AuthenticationException("Invalid password.");
            }

            if (user.Profile.IsActive == false)
            {
                throw new AuthenticationException("Profile deactivated.");
            }

            return user.Profile;
        }

    }

}