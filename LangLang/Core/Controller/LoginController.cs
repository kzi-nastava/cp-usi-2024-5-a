
using LangLang.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;

namespace LangLang.Core.Controller
{

    public class LoginController
    {
        readonly StudentController studentController;
        readonly TutorController tutorController; 
        readonly DirectorController directorController;
        public LoginController(StudentController studentController, TutorController tutorController, DirectorController directorController)
        {
            this.studentController = studentController;
            this.tutorController = tutorController;
            this.directorController = directorController;
        }

        public Profile GetProfileByCredentials(string email, string password)
        {
            try
            {
                var profile = (GetProfile(studentController.GetAll(), email, password)
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