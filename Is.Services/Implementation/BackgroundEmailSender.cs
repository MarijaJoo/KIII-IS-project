using Is.Domain.Email;
using Is.Repository.Interface;
using Is.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Services.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _mailRepository;

        public BackgroundEmailSender(IEmailService es, IRepository<EmailMessage> mr)
        {
            _emailService = es;
            _mailRepository = mr;
        }
        public async Task DoWork()
        {
            await _emailService.SendEmailAsync(_mailRepository.GetAll().Where(z => !z.status).ToList());
        }
    }
}
