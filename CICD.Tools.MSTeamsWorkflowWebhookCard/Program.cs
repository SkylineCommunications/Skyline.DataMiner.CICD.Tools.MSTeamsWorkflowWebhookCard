namespace Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard
{
	using System;

	using System.CommandLine;

	using System.Net.Http;
	using System.Threading.Tasks;

	using Microsoft.Extensions.Logging;

	using Serilog;

	using Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard.Lib;

	/// <summary>
	/// Allows posting adaptive cards to an MSTeams workflow through a webhook.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// Code that will be called when running the tool.
		/// </summary>
		/// <param name="args">Extra arguments.</param>
		/// <returns>0 if successful.</returns>
		public static async Task<int> Main(string[] args)
		{
			var httpPostUrl = new Option<string>(
				name: "--http-post-url",
				description: "The HTTP POST URL as configured in the MSTeams workflow for receiving webhook requests.")
			{
				IsRequired = true,
			};

			var isDebug = new Option<bool>(
				name: "--debug",
				description: "Indicates the tool should write out debug logging.")
			{
				IsRequired = false,
			};

			var rootCommand = new RootCommand("Sends an adaptive card to an MS Teams workflow.")
			{
				httpPostUrl,
				isDebug
			};

			var name = new Option<string>("--name")
			{
				Description = "Name of this pipeline.",
				IsRequired = true
			};

			var result = new Option<CicdResult>("--result")
			{
				Description = "Indicates the result of the CICD run.",
				IsRequired = true
			};

			var details = new Option<string>("--details")
			{
				Description = "Indicates the details of the CICD run. If something went wrong, a short description can be added here.",
				IsRequired = false
			};

			var pathToBuild = new Option<string>("--url-to-build")
			{
				Description = "A URL to the current build that can be opened by the user.",
				IsRequired = false
			};

			var pathToServiceIcon = new Option<string>("--url-to-service-icon")
			{
				Description = "A URL to an icon you want to display on the card.",
				IsRequired = false
			};

			var fromCicd = new Command("from-cicd", "Sends a card formatted as results from a CICD pipeline or workflow.")
			{
				name,
				result,
				details,
				pathToBuild,
				pathToServiceIcon
			};

			rootCommand.AddCommand(fromCicd);

			fromCicd.SetHandler(ProcessFromCicd, isDebug, name, httpPostUrl, result, details, pathToBuild, pathToServiceIcon);

			return await rootCommand.InvokeAsync(args);
		}

		private static async Task<int> ProcessFromCicd(bool isDebug, string name, string httpPostUrl, CicdResult result, string details, string pathToBuild, string iconOfService)
		{
			if (isDebug) Console.WriteLine("Started MSTeamsWorkflowWebHookCard: from cicd.");
			try
			{
				var logConfig = new LoggerConfiguration().WriteTo.Console();
				logConfig.MinimumLevel.Is(isDebug ? Serilog.Events.LogEventLevel.Debug : Serilog.Events.LogEventLevel.Information);
				var seriLog = logConfig.CreateLogger();

				using var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(seriLog));
				var logger = loggerFactory.CreateLogger("Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard");

				using var client = new HttpClient();
				var card = new CicdCard(logger, client);
				card.ApplyConfiguration(name, result, details, pathToBuild, iconOfService);
				await card.SendAsync(httpPostUrl);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return 1;
			}

			if (isDebug) Console.WriteLine("Finished MSTeamsWorkflowWebHookCard: from cicd.");
			return 0;
		}
	}
}