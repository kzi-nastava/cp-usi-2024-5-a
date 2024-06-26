using System;
using System.Net.Mail;
using System.Net;
using System.IO;
using Syncfusion.Pdf;

namespace LangLang.BusinessLogic.UseCases
{
    public class EmailService
    {

        public static void SendEmail(string toEmail, string subject, string body)
        {
            SendEmail(toEmail, subject, body, null);
        }

        public static void SendEmail(string toEmail, string subject, string body, PdfDocument? pdf)
        {
            try
            {
                MailMessage mail = CreateMailMessage(toEmail, subject, body, pdf);
                SmtpClient smtpClient = ConfigureSmtpClient();
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw new SmtpException("Failed to send email: " + ex.Message, ex);
            }
        }

        private static MailMessage CreateMailMessage(string toEmail, string subject, string body, PdfDocument? pdf)
        {
            MailAddress fromAddress = new("langschool5a@gmail.com", "LangLang");
            MailAddress toAddress = new(toEmail);

            MailMessage mail = new();
            mail.From = fromAddress;
            mail.To.Add(toAddress);
            mail.Subject = subject;
            mail.Body = body;

            if (pdf != null)
            {
                var attachment = ConvertFromPdf(pdf);
                mail.Attachments.Add(attachment);
            }

            return mail;
        }

        private static SmtpClient ConfigureSmtpClient()
        {
            return new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("langschool5a@gmail.com", "lckfqhhpgyespyso")
            };
        }

        private static Attachment ConvertFromPdf(PdfDocument document)
        {
            MemoryStream ms = new MemoryStream();
            document.Save(ms);
            document.Close(true);
            ms.Position = 0;
            return new Attachment(ms, "Report", "application/pdf");
        }
    }
}
