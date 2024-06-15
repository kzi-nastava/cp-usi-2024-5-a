using LangLang.Composition;
using LangLang.Configuration;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

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
        public void SendAverageCourseGrades(Director director)
        {
            var reportService = new ReportService();
            var pdfService = new PdfService();

            var reportName = "Average grades and tutor rating per course";
            var headers = new string[] { "Course", "Average knowledge grade", "Average activity grade", "Average tutor rating" };

            var document = PdfService.GeneratePdf<Dictionary<string, List<double>>>(reportService.GetAverageGradesOfCourses(), headers, reportName, data => pdfService.DataToGrid(data));
            EmailService.SendEmail(director.Profile.Email, reportName, "", document);

        }
        public void SendAverageResultsPerSkill(Director director)
        {
            var reportService = new ReportService();
            var pdfService = new PdfService();

            var reportName = "Average exams results per skill in last year";
            var headers = new string[] { "Reading average score", "Writing average score", "Listening average score", "Speaking average score" };
            
            var document = PdfService.GeneratePdf<float[]>(reportService.GetAverageResults(), headers, reportName, data => pdfService.DataToGrid(data));
            EmailService.SendEmail(director.Profile.Email, reportName, "", document);

        }
        public void SendCoursesAccomplishments(Director director)
        {
            var reportService = new ReportService();
            var pdfService = new PdfService();

            var reportName = "Courses enrollments and pass rates analysis";
            var headers = new string[] { "Course", "Total Attendees", "Students Passed", "Pass Rate (%)" };

            var document = PdfService.GeneratePdf<Dictionary<string, float[]>>(reportService.GetCoursesAccomplishment(), headers, reportName, data => pdfService.DataToGrid(data));
            EmailService.SendEmail(director.Profile.Email, reportName, "", document);

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

        public void SendAveragePoints(Director director)
        {
            var reportService = new ReportService();
            var pdfService = new PdfService();

            var reportName = "Average points per language";
            var headers = new string[] { "Language", "Average points" };

            var document = PdfService.GeneratePdf<Dictionary<string, double>>(reportService.GetAveragePoints(), headers, reportName, data => pdfService.DataToGrid(data));
            EmailService.SendEmail(director.Profile.Email, reportName, "", document);
        }

        public void SendAveragePenaltyPoints(Director director)
        {
            var reportService = new ReportService();
            var pdfService = new PdfService();

            var reportName = "Average penalty points per language";
            var headers = new string[] { "Language", "Average penalty points" };

            var document = PdfService.GeneratePdf<Dictionary<string, double>>(reportService.GetAveragePenaltyPoints(), headers, reportName, data => pdfService.DataToGrid(data));
            EmailService.SendEmail(director.Profile.Email, reportName, "", document);
        }

        internal void SendAverageGradeByPenaltyCount(Director director)
        {
            var reportService = new ReportService();
            var pdfService = new PdfService();

            var reportName = "Average grade by penalty count";
            var headers = new string[] { "Language", "Level", "Num of penalties", "Average grade" };

            var document = PdfService.GeneratePdf<Dictionary<(Course, int), double>>(reportService.GetAverageGradeByPenaltyCount(), headers, reportName, data => pdfService.DataToGrid(data));
            EmailService.SendEmail(director.Profile.Email, reportName, "", document);
        }

        internal void SendPenaltiesCountLastYear(Director director)
        {
            var reportService = new ReportService();
            var pdfService = new PdfService();

            var reportName = "Number of penalties per course in last year.";
            var headers = new string[] { "Language", "Num of penalties" };

            var document = PdfService.GeneratePdf<Dictionary<Course, int>>(reportService.GetPenaltiesLastYear(), headers, reportName, data => pdfService.DataToGrid(data));
            EmailService.SendEmail(director.Profile.Email, reportName, "", document);
        }

        public void SentExamsCreated(Director director)
        {
            var reportService = new ReportService();
            var pdfService = new PdfService();

            var reportName = "Number of exams created in last year per language.";
            var headers = new string[] { "Language", "Number of exams created" };

            var document = PdfService.GeneratePdf<Dictionary<string, double>>(reportService.GetNumberOfExams(), headers, reportName, data => pdfService.DataToGrid(data));
            EmailService.SendEmail(director.Profile.Email, reportName, "", document);
        }

        public void SentCoursesCreated(Director director)
        {
            var reportService = new ReportService();
            var pdfService = new PdfService();

            var reportName = "Number of courses created in last year per language.";
            var headers = new string[] { "Language", "Number of courses created" };

            var document = PdfService.GeneratePdf<Dictionary<string, double>>(reportService.GetNumberOfCourses(), headers, reportName, data => pdfService.DataToGrid(data));
            EmailService.SendEmail(director.Profile.Email, reportName, "", document);
        }

        private string[] GetBodyReplacements(ExamResult result, ExamSlot exam)
        {
            var languageService = new LanguageLevelService();
            var language = languageService.Get(exam.LanguageId);

            return new string[] {
            language.Language,
            result.ReadingPoints.ToString(),
            result.WritingPoints.ToString(),
            result.ListeningPoints.ToString(),
            result.SpeakingPoints.ToString(),
            Constants.MIN_TEST_POINTS.ToString()
         };
         }

        private string[] GetSubjectReplacements(ExamSlot exam)
        {
            var languageService = new LanguageLevelService();
            var timeService = new TimeSlotService();

            var language = languageService.Get(exam.LanguageId);
            var timeSlot = timeService.Get(exam.TimeSlotId);

            return new string[]
             {
                timeSlot.Time.ToString("dd.MM.yyyy"),
                language.Language,
                language.Level.ToString()
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
            var langugageService = new LanguageLevelService();
            var language = langugageService.Get(course.LanguageLevelId);
            return new string[]
            {
                language.Language,
                language.Level.ToString(),
                language.Level.ToString()
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
