using Microsoft.Extensions.Configuration;
using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.implement
{
    public class EmailSender : ISendMailProvider
    {
        private readonly IConfiguration _config;
        
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string message, bool isHtml = false, string mailCc = null)
        {
            var msg = new MailMessage
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml
            };

            var emails = email.Split(",");
            foreach (var em in emails)
            {
                msg.To.Add(new MailAddress(em));
            }

            if (!string.IsNullOrEmpty(mailCc))
            {
                var emailCc = mailCc.Split(",");
                foreach (var em in emailCc)
                {
                    msg.CC.Add(new MailAddress(em));
                }
            }

            await DoSendEmailAsync(msg);
        }

        public async Task SendTemplateEmailAsync(string emails, string templateId, string subject, object templateData, string mailCc = null)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplate", $"{templateId}.html");
            var html = File.ReadAllText(filePath);
            var template = Template.Parse(html);
            var body = await template.RenderAsync(templateData, m => m.Name);
            await SendEmailAsync(emails, subject, body, true, mailCc);
        }

        #region Private Methods
        private async Task DoSendEmailAsync(MailMessage message)
        {
            message.From = new MailAddress(_config["SMTP:From"]);
            using (var client = new SmtpClient(_config["SMTP:Host"], int.Parse(_config["SMTP:Port"]))
            {
                Credentials = new NetworkCredential(_config["SMTP:Username"], _config["SMTP:Password"]),
                EnableSsl = true
            })
            {
                await client.SendMailAsync(message);
            }
        }
        #endregion
    }

}
