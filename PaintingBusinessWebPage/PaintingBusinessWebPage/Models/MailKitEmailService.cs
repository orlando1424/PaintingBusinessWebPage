using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;

namespace PaintingBusinessWebPage.Models
{
    public class MailKitEmailService : IEmailService
    {
        private readonly EmailServerConfiguration _econfig;

        public MailKitEmailService(EmailServerConfiguration config)
        {
            _econfig = config;
        }
        public void Send(EmailMessage msg)
        {
            var message = new MimeMessage();
            message.To.AddRange(msg.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(msg.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = msg.Subject;

            message.Body = new TextPart("plain")
            {
                Text = msg.Content
        };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(_econfig.SmtpServer, _econfig.SmtpPort);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_econfig.SmtpUsername, _econfig.SmtpPassword);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
