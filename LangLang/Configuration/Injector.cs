using System.Collections.Generic;
using System;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LangLang.Composition
{
    public class Injector
    {

        private static ServiceProvider _serviceProvider;
        public static void SetServiceProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IStudentRepository), new StudentRepository() },
            { typeof(IDirectorRepository), new DirectorRepository() },
            { typeof(IEnrollmentRequestRepository), new EnrollmentRequestRepository() },
            { typeof(IWithdrawalRequestRepository), new WithdrawalRequestRepository() },
            { typeof(ITutorRatingRepository), new TutorRatingRepository() },
            { typeof(IExamApplicationRepository), new ExamApplicationRepository() },
            { typeof(IPenaltyPointRepository), new PenaltyPointRepository() },
            { typeof(IGradeRepository), new GradeRepository() },
            { typeof(IExamResultRepository), new ExamResultRepository() },
            { typeof(IEmailRepository), new EmailRepository() }
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }
            else
            {
                return _serviceProvider.GetRequiredService<T>();
            }
            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
