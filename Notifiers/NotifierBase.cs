using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nml.Refactor.Me.Notifiers
{
    public abstract class NotifierBase<T>: INotifier
    {
        protected readonly IMessageBuilder<T> _messageBuilder;
        protected readonly IOptions _options;
        protected readonly ILogger _logger;
        public NotifierBase(IMessageBuilder<T> messageBuilder, IOptions options, ILogger logger)
        {
            _messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
        }
		public async Task Notify(NotificationMessage message)
		{
			var mailMessage = _messageBuilder.CreateMessage(message);

			try
			{
				await SendMessage(mailMessage, message);
			}
			catch (AggregateException e)
			{
				foreach (var exception in e.Flatten().InnerExceptions)
					_logger.LogError(exception, $"Failed to send message. {exception.Message}");

				throw;
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Failed to send message. {e.Message}");
				throw;
			}
		}
		protected abstract Task SendMessage(T message, NotificationMessage notification);

    }
}
