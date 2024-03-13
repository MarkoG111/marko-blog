using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer
{
    public class SendEmailDto
    {
        public string Content { get; set; }
        public string Subject { get; set; }
        public string SendTo { get; set; }
    }
}