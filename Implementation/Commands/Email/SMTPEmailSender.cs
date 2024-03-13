using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.Commands.Email;
using Application.DataTransfer;

using System.Net;
using System.Net.Mail;

namespace Implementation.Commands.Email
{
    public class SMTPEmailSender : IEmailSender
    {
        public void Send(SendEmailDto dto)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("blogapiasp@gmail.com", "tmoe xbbp gxmn jeco")
            };

            var message = new MailMessage("blogapiasp@gmail.com", dto.SendTo);
            message.Subject = dto.Subject;
            message.Body = dto.Content;
            message.IsBodyHtml = true;

            smtp.Send(message);
        }
    }
}