using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Emails;

namespace Application.Commands.Email
{
    public interface IEmailSender
    {
        void Send(SendEmailDto dto);
    }
}