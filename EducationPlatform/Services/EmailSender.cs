using EducationPlatform.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MailKit.Security;

namespace EducationPlatform.Services
{
    public class EmailSender : IEmailSender
    {
        private string _title;
        private string _email;
        private string _password;
        private string _host;
        private int _port;

        public EmailSender(IConfiguration configuration)
        {
            var mail = configuration.GetSection("Mail");

            _title = mail.GetValue<string>("Title");
            _email = mail.GetValue<string>("Address");
            _password = mail.GetValue<string>("Password");
            _host = mail.GetValue<string>("Host");
            _port = mail.GetValue<int>("Port");
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            MimeMessage emailMessage = CreateMimeMessage(email, subject, message);

            using (var client = new SmtpClient())
            {
                try
                {
                    // during development
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTlsWhenAvailable);
                    await client.AuthenticateAsync(_email, _password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                catch (Exception)
                { }
            }
        }

        private MimeMessage CreateMimeMessage(string email, string subject, string message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_title, _email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            return emailMessage;
        }
    }
}
