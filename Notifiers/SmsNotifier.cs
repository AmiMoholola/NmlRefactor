using System;
using System.Threading.Tasks;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class SmsNotifier : NotifierBase<string>
	{
		public SmsNotifier(IStringMessageBuilder messageBuilder, IOptions options):
			base(messageBuilder, options, LogManager.For<EmailNotifier>())
		{
		}

        protected async override Task SendMessage(string message, NotificationMessage notification)
        {
			var smsClient = new SmsApiClient(_options.Sms.ApiUri, _options.Sms.ApiKey);
			await smsClient.SendAsync(notification.To, message);
			_logger.LogTrace($"Message sent.");
		}
	}
}
