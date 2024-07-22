namespace CICD.Tools.MSTeamsWorkflowWebhookCard.LibTests
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using CICD.Tools.MSTeamsWorkflowWebhookCard.LibTests.MatchTeamsRequirement;

    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RichardSzalay.MockHttp;

    using Serilog;

    using Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard.Lib;

    [TestClass]
    public class CicdCardTests
    {
        [TestMethod]
        public async Task SendAsyncTest_TestFormatIsValidForTeams()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();

            var logConfig = new LoggerConfiguration().WriteTo.Console();
            logConfig.MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug);
            var seriLog = logConfig.CreateLogger();

            using (var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(seriLog)))
            {
                var logger = loggerFactory.CreateLogger("Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard");

                const string name = "TestPipeline";
                const CicdResult result = CicdResult.Success;
                const string details = "Some Details";
                const string pathToBuild = "https://skyline.be/skyline/about";
                const string iconOfService = "https://skyline.be/skylicons/duotone/SkylineLogo_Duo_Light.png";
                const string url = "https://skyline.be/";

                var matcher = new TeamsAdaptiveCardMatcher(logger);
                var responseContent = new StringContent("OK", Encoding.UTF8, "application/string");
                mockHttp.When(HttpMethod.Post, url).With(matcher).Respond(System.Net.HttpStatusCode.OK, responseContent);

                using (var client = mockHttp.ToHttpClient())
                {
                    // Act
                    var card = new CicdCard(logger, client);
                    card.ApplyConfiguration(name, result, details, pathToBuild, iconOfService);
                    await card.SendAsync(url);

                    // Assert
                    mockHttp.VerifyNoOutstandingExpectation();
                }
            }
        }

        [TestMethod, Ignore]
        public async Task SendAsyncTest_IntegrationTest()
        {
            // Arrange
            var logConfig = new LoggerConfiguration().WriteTo.Console();
            logConfig.MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug);
            var seriLog = logConfig.CreateLogger();

            using (var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(seriLog)))
            {
                var logger = loggerFactory.CreateLogger("Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard");

                const string name = "IntegrationTestPipeline";
                const CicdResult result = CicdResult.Success;
                const string details = "This is an integration test running from the testbattery. \r\n This should be on a second line!";
                const string pathToBuild = "https://github.com/SkylineCommunications/Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard";
                const string iconOfService = "https://skyline.be/skylicons/duotone/SkylineLogo_Duo_Light.png";
                const string url = "EnterWebHookFromTeamsChannelHere";

                using (HttpClient client = new HttpClient())
                {
                    // Act
                    var card = new CicdCard(logger, client);
                    card.ApplyConfiguration(name, result, details, pathToBuild, iconOfService);
                    await card.SendAsync(url);

                    // Assert

                    // Manual Verification of content as this is Graphical Design.
                    Assert.IsTrue(true);
                }
            }
        }
    }
}