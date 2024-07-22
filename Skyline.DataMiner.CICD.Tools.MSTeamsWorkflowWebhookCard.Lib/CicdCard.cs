namespace Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard.Lib
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using AdaptiveCards;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a CICD card for MS Teams webhook integration.
    /// </summary>
    public class CicdCard
    {
        private readonly AdaptiveCard card;
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CicdCard"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="httpClient">The HTTP client instance.</param>
        public CicdCard(ILogger logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            card = new AdaptiveCard("1.2");
        }

        /// <summary>
        /// Applies configuration to the adaptive card.
        /// </summary>
        /// <param name="nameOfPipeline">Name of the pipeline.</param>
        /// <param name="result">Result of the CICD process.</param>
        /// <param name="detailsOfResult">Details of the result.</param>
        /// <param name="pathToPipelineRun">Path to the pipeline run.</param>
        /// <param name="iconOfService">Icon of the service.</param>
        public void ApplyConfiguration(string nameOfPipeline, CicdResult result, string detailsOfResult = null, string pathToPipelineRun = null, string iconOfService = null)
        {
            logger.LogDebug("Creating Card...");

            const string iconOk = "https://skyline.be/skylicons/duotone/SuccesfullIdea_Duo_Light.png";
            const string iconUnstable = "https://skyline.be/skylicons/duotone/Warning_Duo_Light.png";
            const string iconWarning = "https://skyline.be/skylicons/duotone/FaultManager_Duo_Light.png";
            const string iconFailure = "https://skyline.be/skylicons/duotone/Forbidden_Duo_Light.png";

            Uri statusIcon;
            switch (result)
            {
                case CicdResult.Success:
                    statusIcon = new Uri(iconOk);
                    break;

                case CicdResult.Unstable:
                    statusIcon = new Uri(iconUnstable);
                    break;

                case CicdResult.Failure:
                    statusIcon = new Uri(iconFailure);
                    break;

                case CicdResult.Warning:
                    statusIcon = new Uri(iconWarning);
                    break;

                default:
                    statusIcon = new Uri(iconOk);
                    break;
            }

            List<AdaptiveElement> cardElements = new List<AdaptiveElement>
            {
                new AdaptiveColumnSet
                {
                    Columns = new List<AdaptiveColumn>
                    {
                        new AdaptiveColumn
                        {
                            Width = "stretch",
                            Items = new List<AdaptiveElement>
                            {
                                new AdaptiveTextBlock
                                {
                                    Text = $"{nameOfPipeline}: {result}",
                                    Size = AdaptiveTextSize.Large,
                                    Weight = AdaptiveTextWeight.Bolder,
                                    Wrap = true
                                }
                            }
                        },
                        new AdaptiveColumn
                        {
                            Width = "auto",
                            Items = new List<AdaptiveElement>
                            {
                                new AdaptiveImage
                                {
                                    Url = statusIcon,
                                    Size = AdaptiveImageSize.Small,
                                    AltText = "Status Icon"
                                }
                            }
                        }
                    }
                }
            };

            if (!String.IsNullOrWhiteSpace(iconOfService))
            {
                logger.LogDebug("Icon provided, adding...");
                cardElements.Add(new AdaptiveImage
                {
                    Url = new Uri(iconOfService),
                    Size = AdaptiveImageSize.Medium
                });
            }
            else
            {
                logger.LogDebug("No icon provided, skipping...");
            }

            if (!String.IsNullOrWhiteSpace(pathToPipelineRun))
            {
                logger.LogDebug("Path to build provided, adding...");
                cardElements.Add(new AdaptiveActionSet
                {
                    Actions = new List<AdaptiveAction>
                    {
                        new AdaptiveOpenUrlAction
                        {
                            Title = "View Build",
                            Url = new Uri(pathToPipelineRun)
                        }
                    }
                });
            }
            else
            {
                logger.LogDebug("No path to build provided, skipping...");
            }

            cardElements.Add(new AdaptiveFactSet
            {
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Status", result.ToString())
                }
            });

            if (!String.IsNullOrWhiteSpace(detailsOfResult))
            {
                logger.LogDebug("Details provided, adding...");

                string formatedDetails = detailsOfResult.Replace("\r\n", "\n")
                                                        .Replace("\n\n", "\n")
                                                        .Replace("\n", "\n\n");

                cardElements.Add(new AdaptiveTextBlock
                {
                    Text = formatedDetails,
                    Wrap = false,
                    IsSubtle = false,
                    Separator = true
                });
            }
            else
            {
                logger.LogDebug("No details provided, skipping...");
            }

            card.Body = cardElements;
        }

        /// <summary>
        /// Sends the adaptive card to the specified webhook URL asynchronously.
        /// </summary>
        /// <param name="httpPostUrl">The URL to post the card to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendAsync(string httpPostUrl)
        {
            logger.LogDebug("Serializing card to JSON...");
            var cardJson = JsonConvert.SerializeObject(new
            {
                type = "message",
                attachments = new[]
                {
                    new
                    {
                        contentType = "application/vnd.microsoft.card.adaptive",
                        content = card
                    }
                }
            });

            logger.LogDebug(cardJson);
            logger.LogDebug("Sending HTTP Post to webhook...");
            var requestContent = new StringContent(cardJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(httpPostUrl, requestContent);
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully sent the card to MS Teams.");
            }
            else
            {
                logger.LogError($"Failed to send the card to MS Teams. Status code: {response.StatusCode}. Details: {response.Content}");
            }
        }
    }
}