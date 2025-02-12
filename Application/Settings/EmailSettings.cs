using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Settings
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = Environment.GetEnvironmentVariable("SMTP_SERVER");
        public int SmtpPort { get; set; } = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
        public string SenderEmail { get; set; } = Environment.GetEnvironmentVariable("SENDER_EMAIL");
        public string SenderPassword { get; set; } = Environment.GetEnvironmentVariable("SENDER_PASSWORD");
    }
}