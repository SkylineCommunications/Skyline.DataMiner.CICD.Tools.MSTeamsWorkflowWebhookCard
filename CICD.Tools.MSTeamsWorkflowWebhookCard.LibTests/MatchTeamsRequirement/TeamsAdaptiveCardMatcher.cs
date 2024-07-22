namespace Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard.Lib.Tests
{
	using System;
	using System.Linq;
	using System.Net.Http;

	using Microsoft.Extensions.Logging;

	using Newtonsoft.Json;

	using RichardSzalay.MockHttp;

	/// <summary>
	/// Matches HTTP POST requests to expected Teams adaptive card format.
	/// Checks the structure of the request but not the card content.
	/// </summary>
	public class TeamsAdaptiveCardMatcher : IMockedRequestMatcher
	{
		private readonly ILogger logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="TeamsAdaptiveCardMatcher"/> class.
		/// </summary>
		/// <param name="logger">The logger instance.</param>
		public TeamsAdaptiveCardMatcher(ILogger logger)
		{
			this.logger = logger;
		}

		/// <summary>
		/// Determines whether the specified HTTP request message matches the expected Teams adaptive card format.
		/// </summary>
		/// <param name="message">The HTTP request message to check.</param>
		/// <returns>True if the request matches; otherwise, false.</returns>
		public bool Matches(HttpRequestMessage message)
		{
			try
			{
				if (message == null)
				{
					logger.LogError("message is null");
					return false;
				}

				if (message.Content == null)
				{
					logger.LogError("message.Content is null");
					return false;
				}

				var rootString = message.Content.ReadAsStringAsync().Result;

				if (String.IsNullOrEmpty(rootString))
				{
					logger.LogError("rootString is null or empty");
					return false;
				}

				var rootObject = JsonConvert.DeserializeObject<Message>(rootString);

				if (rootObject == null)
				{
					logger.LogError("rootObject is null");
					return false;
				}

				if (rootObject.Type != "message")
				{
					logger.LogError($"rootObject.Type {rootObject.Type} does not match 'message'");
					return false;
				}

				if (rootObject.Attachments == null)
				{
					logger.LogError("rootObject.Attachments is null");
					return false;
				}

				if (rootObject.Attachments.Count != 1)
				{
					logger.LogError($"rootObject.Attachments.Count {rootObject.Attachments.Count} does not match 1");
					return false;
				}

				var attachment = rootObject.Attachments.FirstOrDefault();

				if (attachment == null)
				{
					logger.LogError("First attachment is null");
					return false;
				}

				if (attachment.ContentType != "application/vnd.microsoft.card.adaptive")
				{
					logger.LogError($"attachment.ContentType {attachment.ContentType} does not match 'application/vnd.microsoft.card.adaptive'");
					return false;
				}

				if (attachment.Content == null)
				{
					logger.LogError("Content is null");
					return false;
				}

				if (attachment.Content.Type == null)
				{
					logger.LogError("Content.Type is null");
					return false;
				}

				if (attachment.Content.Body == null)
				{
					logger.LogError("Content.Body is null");
					return false;
				}

				if (attachment.Content.Type != "AdaptiveCard")
				{
					logger.LogError($"attachment.Content.Type {attachment.Content.Type} does not match 'AdaptiveCard'");
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return false;
			}
		}
	}
}