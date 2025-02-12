using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Email;
using Application.DataTransfer.Emails;
using Application.Settings;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Implementation.Commands.Email
{
    public class SMTPEmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public SMTPEmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void Send(SendEmailDto dto)
        {
            var smtp = new SmtpClient
            {
                Host = _emailSettings.SmtpServer,
                Port = _emailSettings.SmtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                // Allow less security apps
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword)
            };

            var message = new MailMessage(_emailSettings.SenderEmail, dto.SendTo)
            {
                Subject = dto.Subject,
                Body = dto.Content,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }
    }
}