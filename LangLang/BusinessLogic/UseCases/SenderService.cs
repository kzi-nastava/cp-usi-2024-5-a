using LangLang.Configuration;
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using LangLang.Utilities;
using System.Collections.Generic;
using System.IO;

namespace LangLang.BusinessLogic.UseCases
{
    public class SenderService
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

    }
}
