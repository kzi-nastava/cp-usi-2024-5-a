using System.Collections.Generic;
using System;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories;

namespace LangLang.Composition
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IStudentRepository), new StudentRepository() },
            { typeof(ITutorRepository), new TutorRepository() },
            { typeof(IDirectorRepository), new DirectorRepository() },
            { typeof(IEnrollmentRequestRepository), new EnrollmentRequestRepository() }
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
