using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
    public class EmailNotifier : NotifierBase<MailMessage>
    {
        public EmailNotifier(IMailMessageBuilder messageBuilder, IOptions options) :
            base(messageBuilder, options, LogManager.For<EmailNotifier>())
        {
        }

        protected async override Task SendMessage(MailMessage mailMessage, NotificationMessage notification)
        {
            var smtp = new SmtpClient(_options.Email.SmtpServer);
            smtp.Credentials = new NetworkCredential(_options.Email.UserName, _options.Email.Password);
            await smtp.SendMailAsync(mailMessage);
            _logger.LogTrace($"Message sent.");
        }
    }
}
