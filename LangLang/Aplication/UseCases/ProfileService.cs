
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;
using System.Collections.Generic;

namespace LangLang.Aplication.UseCases
{
    public class ProfileService
    {

        public bool EmailExists(string email)
        {
            var studentService = new StudentService();
            var tutorService = new TutorService();
            var directorService = new DirectorService();
            if (EmailExistsInList(studentService.GetAll(), email)) return true;
            if (EmailExistsInList(tutorService.GetAll(), email)) return true;
            if (EmailExistsInList(directorService.GetAll(), email)) return true;

            return false;
        }


        public bool EmailExists(string email, int id, UserType role)
        {
            var studentService = new StudentService();
            var tutorService = new TutorService();
            var directorService = new DirectorService();

            if (role == UserType.Student)
                return EmailExistsInList(studentService.GetAll(), email, id);

            if (role == UserType.Tutor)
                return EmailExistsInList(tutorService.GetAll(), email, id);

            return EmailExistsInList(directorService.GetAll(), email, id);
        }

        private bool EmailExistsInList<T>(List<T> list, string email) where T : IProfileHolder
        {
            foreach (T item in list)
            {
                if (item.Profile.Email == email) return true;
            }
            return false;
        }

        private bool EmailExistsInList<T>(IEnumerable<T> list, string email, int id) where T : IProfileHolder
        {
            foreach (T item in list)
            {
                if (item.Profile.Email == email && item.Profile.Id != id) return true;
            }
            return false;
        }

        public Profile? GetProfile(int id, UserType role)
        {
            var studentService = new StudentService();
            var tutorService = new TutorService();
            var directorService = new DirectorService();

            return role switch
            {
                UserType.Student => studentService.Get(id)?.Profile,
                UserType.Tutor => tutorService.Get(id)?.Profile,
                UserType.Director => directorService.GetAll()[0].Profile,
                _ => null,
            };
        }
    }
}

