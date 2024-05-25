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
            { typeof(IEnrollmentRequestRepository), new EnrollmentRequestRepository() },
            { typeof(IWithdrawalRequestRepository), new WithdrawalRequestRepository() },
            { typeof(ITutorRatingRepository), new TutorRatingRepository() },
            { typeof(IExamSlotRepository), new ExamSlotRepository() },
            { typeof(IExamApplicationRepository), new ExamApplicationRepository() },
            { typeof(IPenaltyPointRepository), new PenaltyPointRepository() },
            { typeof(ICourseRepository), new CourseRepository() },
            { typeof(IGradeRepository), new GradeRepository() }

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
