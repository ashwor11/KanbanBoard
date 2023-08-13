using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Core.Security.Mailing.Mailkit
{
    public class MailkitService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailkitService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmail(Mail mail)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(mail.ToFullName, mail.ToEmail));

            email.Subject = mail.Subject;

            BodyBuilder bodyBuilder = new()
            {
                TextBody = mail.TextBody,
                HtmlBody = mail.HtmlBody
            };

            if (mail.Attachments != null)
            {
                foreach (MimeEntity mailAttachment in mail.Attachments)
                {
                    bodyBuilder.Attachments.Add(mailAttachment);
                }
            }

            email.Body = bodyBuilder.ToMessageBody();

            using SmtpClient smtpClient = new();

            await smtpClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.Auto);
            await smtpClient.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);
            await smtpClient.SendAsync(email);
            await smtpClient.DisconnectAsync(true);

        }
    }
}
