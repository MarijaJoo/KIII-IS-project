using Is.Domain.Email;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(List<EmailMessage> message);
    }
}
