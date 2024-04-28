
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
        // TODO: add director
        public LoginController(StudentController studentController, TutorController tutorController)
        {
            this.studentController = studentController;
            this.tutorController = tutorController;
        }

        public Profile GetProfileByCredentials(string email, string password)
        {
            try
            {
                var profile = (GetProfile(studentController.GetAllStudents(), email, password)
                              ?? GetProfile(tutorController.GetAllTutors(), email, password)) 
                              ?? throw new AuthenticationException("Invalid email address");
                return profile; // profile with the given credentials exists
            }
            catch (AuthenticationException ex)
            {
                throw new AuthenticationException(ex.Message); // email exists but password is incorrect
            }
        }

        private Profile? GetProfile<userType>(Dictionary<int, userType> users, string email, string password) where userType: IProfileHolder
        {
            userType user = users.FirstOrDefault(user => user.Value.Profile.Email == email).Value;

            if (user == null || user.Profile == null)
            {
                return null; // It might be in another container, so we don't throw an exception here.
            }

            if (user.Profile.Password != password)
            {
                throw new AuthenticationException("Invalid password");
            }

            return user.Profile;
        }
        
    }

}