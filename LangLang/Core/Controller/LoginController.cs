
using LangLang.Aplication.UseCases;
using LangLang.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;

namespace LangLang.Core.Controller
{

    public class LoginController
    {
        readonly TutorController tutorController; 
        readonly DirectorController directorController;
        public LoginController(TutorController tutorController, DirectorController directorController)
        {
            this.tutorController = tutorController;
            this.directorController = directorController;
        }

        public Profile GetProfileByCredentials(string email, string password)
        {
            var studentService = new StudentService();
            try
            {
                var profile = (GetProfile(studentService.GetAll(), email, password)
                              ?? GetProfile(tutorController.GetAll(), email, password)) 
                              ?? GetProfile(directorController.GetAll(), email, password)
                              ?? throw new AuthenticationException("Invalid email address.");
                return profile; // profile with the given credentials exists
            }
            catch (AuthenticationException ex)
            {
                throw new AuthenticationException(ex.Message); // email exists but password is incorrect
            }
        }

        private Profile? GetProfile<userType>(List<userType> users, string email, string password) where userType: IProfileHolder
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

            if (user.Profile.IsActive == true)
            {
                throw new AuthenticationException("Profile deactivated.");
            }

            return user.Profile;
        }
        
    }

}