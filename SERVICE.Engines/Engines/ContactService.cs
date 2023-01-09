using CORE.ApplicationCommon.Helpers;
using CORE.ApplicationCore.UnitOfWork;
using GazeteKapiMVC5Core.DataAccessLayer;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace SERVICE.Engine.Engines
{
    public class ContactService : IContactService
    {
        private readonly EmailConfiguration emailConfig;
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public ContactService(IUnitOfWork<NewsAppContext> unitOfWork, IOptions<EmailConfiguration> emailConfiguration)
        {
            _unitOfWork = unitOfWork;
            emailConfig = emailConfiguration.Value;
        }
        public async Task<string> SendFormToSubscribe(EmailConfig config)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(MailboxAddress.Parse(emailConfig.From));
                emailMessage.To.Add(MailboxAddress.Parse(config.emailAdress));
                emailMessage.Subject = config.subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<div>{0}</div>", config.content) };

                using (var client = new SmtpClient())
                {
                    client.CheckCertificateRevocation = false;
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(emailConfig.Host, emailConfig.Port, false);
                    await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
                    await client.SendAsync(emailMessage);
                }
                return "Başarılı!";
            }
            catch (Exception ex)
            {
                return ex.ToString(); 
            }

           
        }
    }
}
