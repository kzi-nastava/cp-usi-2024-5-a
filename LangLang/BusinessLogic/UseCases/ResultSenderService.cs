using LangLang.Configuration;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.Utilities;
using System.Collections.Generic;

namespace LangLang.BusinessLogic.UseCases
{
    public class ResultSenderService
    {

        public void SendResults(ExamSlot exam)
        {
            var resultService = new ExamResultService();
            var studentService = new StudentService();
            var emailService = new EmailService();

            List<ExamResult> results = resultService.GetByExam(exam);
            foreach (ExamResult result in results)
            {
                Student student = studentService.Get(result.StudentId);

                string subject = emailService.GetSubject();
                string body = GetBody(result);

                body = Utils.ReplacePlaceholders(body, GetBodyReplacements(result, exam));
                subject = Utils.ReplacePlaceholders(subject, GetSubjectReplacements(exam));

                EmailService.SendEmail(student.Profile.Email, subject, body);
            }
        }

        public void SendGratitudeMail(Course course, List<Student> students)
        {
            var emailService = new EmailService();

            foreach (var student in students)
            {
                string subject = emailService.GetGratitudeSubject();
                string body = emailService.GetGratitudeMessage();

                subject = Utils.ReplacePlaceholders(subject, GetSubjectReplacements(course));
                body = Utils.ReplacePlaceholders(body, GetBodyReplacements(student));

                EmailService.SendEmail(student.Profile.Email, subject, body);
            }
        }

        private string[] GetBodyReplacements(ExamResult result, ExamSlot exam)
        {
            return new string[] {
            exam.Language,
            result.ReadingPoints.ToString(),
            result.WritingPoints.ToString(),
            result.ListeningPoints.ToString(),
            result.SpeakingPoints.ToString(),
            Constants.MIN_TEST_POINTS.ToString()
         };
         }

        private string[] GetSubjectReplacements(ExamSlot exam)
        {
            return new string[]
             {
                exam.TimeSlot.Time.ToString("dd.MM.yyyy"),
                exam.Language,
                exam.Level.ToString(),
             };
        }

        private string GetBody(ExamResult result)
        {
            var emailService = new EmailService();

            if (result.Outcome == ExamOutcome.Passed)
                return emailService.GetPassingMessage();
            else
                return emailService.GetFailingMessage();
        }

        private string[] GetSubjectReplacements(Course course)
        {
            return new string[]
            {
                course.Language,
                course.Level.ToString()
            };
        }

        private string[] GetBodyReplacements(Student student)
        {
            return new string[]
            {
                student.Profile.Name,
                student.Profile.LastName
            };
        }

    }
}
