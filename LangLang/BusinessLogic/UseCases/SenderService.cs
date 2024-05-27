﻿using LangLang.Composition;
using LangLang.Configuration;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Utilities;
using System.Collections.Generic;

namespace LangLang.BusinessLogic.UseCases
{
    public class SenderService
    {
        private IEmailRepository _emails;

        public SenderService()
        {
            _emails = Injector.CreateInstance<IEmailRepository>();
        }

        public void SendResults(ExamSlot exam)
        {
            var resultService = new ExamResultService();
            var studentService = new StudentService();

            List<ExamResult> results = resultService.GetByExam(exam);
            foreach (ExamResult result in results)
            {
                Student student = studentService.Get(result.StudentId);

                string subject = GetSubject();
                string body = GetBody(result);

                body = Utils.ReplacePlaceholders(body, GetBodyReplacements(result, exam));
                subject = Utils.ReplacePlaceholders(subject, GetSubjectReplacements(exam));

                EmailService.SendEmail(student.Profile.Email, subject, body);
            }
        }

        public void SendGratitudeMail(Course course, List<Student> students)
        {
            foreach (var student in students)
            {
                string subject = GetGratitudeSubject();
                string body = GetGratitudeMessage();

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
            if (result.Outcome == ExamOutcome.Passed)
                return GetPassingMessage();
            else
                return GetFailingMessage();
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

        private string GetSubject()
        {
            return _emails.GetContent("resultSubject");
        }

        private string GetFailingMessage()
        {
            return _emails.GetContent("failingMessage");
        }

        private string GetPassingMessage()
        {
            return _emails.GetContent("passingMessage");
        }

        private string GetGratitudeMessage()
        {
            return _emails.GetContent("gratitudeMessage");
        }

        private string GetGratitudeSubject()
        {
            return _emails.GetContent("gratitudeSubject");
        }

    }
}
