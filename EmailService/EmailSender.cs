using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace ToDoProject.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly string _apiKey;
        public EmailSender(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(_apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("cristianoaloigi0@gmail.com", "Cristiano Aloigi (Admin)"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }

        //private MimeMessage CreateEmailMessage(Message message)
        //{
        //    var emailMessage = new MimeMessage();
        //    emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.From));
        //    emailMessage.To.AddRange(message.To);
        //    emailMessage.Subject = message.Subject;

        //    var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };

        //    if (message.Attachments != null && message.Attachments.Any())
        //    {
        //        byte[] fileBytes;
        //        foreach (var attachment in message.Attachments)
        //        {
        //            using (var ms = new MemoryStream())
        //            {
        //                attachment.CopyTo(ms);
        //                fileBytes = ms.ToArray();
        //            }

        //            bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
        //        }
        //    }

        //    emailMessage.Body = bodyBuilder.ToMessageBody();
        //    return emailMessage;
        //}
    }
}
