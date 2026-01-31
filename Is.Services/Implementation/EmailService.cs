using Is.Domain.Email;
using Is.Services.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using MimeKit;
using MailKit.Security;
using System.Linq;
using System.Threading.Tasks;

namespace Is.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _mailSettings;

        public EmailService(IOptions<EmailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(List<EmailMessage> allMails)
        {
            List<MimeMessage> messages = new List<MimeMessage>();
            foreach (var item in allMails)
            {
                var email = new MimeMessage
                {
                    Sender = new MailboxAddress(_mailSettings.SendersName, _mailSettings.SmtpUserName),
                    Subject = item.Subject
                };
                email.From.Add(new MailboxAddress(_mailSettings.EmailDisplayName, _mailSettings.SmtpUserName));
                email.To.Add(new MailboxAddress(item.MailTo, item.MailTo));
                email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = item.Content };
                messages.Add(email);
            }



            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    //var socketOptions = SecureSocketOptions.Auto;

                    var socketOptions = _mailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;

                    await smtp.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.SmtpServerPort, socketOptions);

                    if (!string.IsNullOrEmpty(_mailSettings.SmtpUserName))
                    {
                        await smtp.AuthenticateAsync(_mailSettings.SmtpUserName, _mailSettings.SmtpPassword);
                    }
                    foreach (var item in messages)
                    {
                        await smtp.SendAsync(item);
                    }
                    
                    await smtp.DisconnectAsync(true);
                }

            }
            catch (SmtpException ex)
            {
                throw ex;
            }

    }
    }
}
