using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class TeamsNotifier : NotifierBase<JObject>
	{
		public TeamsNotifier(IWebhookMessageBuilder messageBuilder, IOptions options):
			base(messageBuilder, options, LogManager.For<EmailNotifier>())
		{
		}

        protected async override Task SendMessage(JObject message, NotificationMessage notification)
        {
			var serviceEndPoint = new Uri(_options.Teams.WebhookUri);
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, serviceEndPoint);
			request.Content = new StringContent(message.ToString(),
				Encoding.UTF8,
				"application/json");
			var response = await client.SendAsync(request);
			_logger.LogTrace($"Message sent. {response.StatusCode} -> {response.Content}");
		}
	}
}
