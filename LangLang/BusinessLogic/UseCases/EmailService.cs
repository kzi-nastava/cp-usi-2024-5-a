using System;
using System.Net.Mail;
using System.Net;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Composition;
using System.IO;
using Syncfusion.Pdf;

namespace LangLang.BusinessLogic.UseCases
{
    public class EmailService
    {

        private IEmailRepository _emails;

        public EmailService()
        {
            _emails = Injector.CreateInstance<IEmailRepository>();
        }

        public static void SendEmail(string toEmail, string subject, string body)
        {
            SendEmail(toEmail, subject, body, null);
        }

        public static void SendEmail(string toEmail, string subject, string body, PdfDocument? pdf)
        {
            try
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

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com", 
                    Port = 587,
                    EnableSsl = true, 
                    Credentials = new NetworkCredential("langschool5a@gmail.com", "lckfqhhpgyespyso")
                };

                smtpClient.Send(mail);
                
            }
            catch (Exception ex)
            {
                throw new SmtpException(ex.Message);
            }
        }

        public string GetSubject()
        {
            return _emails.GetContent("subject");  
        }

        public string GetFailingMessage()
        {
            return _emails.GetContent("failingMessage");
        }

        public string GetPassingMessage()
        {
            return _emails.GetContent("passingMessage");
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
