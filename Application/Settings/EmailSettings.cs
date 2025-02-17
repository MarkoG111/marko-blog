using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Settings
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }

        public EmailSettings()
        {
            SmtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? throw new ArgumentNullException(nameof(SmtpServer), "SMTP_SERVER env variable is required");
            SmtpPort = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out int port) ? port : 587;
            SenderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL") ?? throw new ArgumentNullException(nameof(SenderEmail), "SENDER_EMAIL env variable is required");
            SenderPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD") ?? throw new ArgumentNullException(nameof(SenderPassword), "SENDER_PASSWORD env variable is required");
        }
    }
}