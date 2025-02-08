using Client.BuildingBlocks.Http;
using Microsoft.AspNetCore.Components;
using Shared;
using Shared.ChatMessages;
using Shared.MarketResearch;
using Shared.YouTube;
using System.Text.RegularExpressions;

namespace Client.BuildingBlocks.Modals
{
    public partial class VideoOverviewModal : ComponentBase
    {
        [Parameter]
        public EventCallback ModalExitedCallback { get; set; }

        [Inject]
        public HttpClientService HttpClientService { get; set; }

        [Parameter]
        public VideoProposalDTO VideoProposal { get; set; }

        [Parameter]
        public MarketResearchDTO MarketResearch { get; set; }
        private bool requestOngoing;
        public async Task CloseModalAsync()
        {
            if (ModalExitedCallback.HasDelegate)
            {
                await ModalExitedCallback.InvokeAsync(this);
            }
        }

        private string GetYouTubeVideoId(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            // Regular expression to extract YouTube video ID
            var regex = new Regex(@"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^""&?\/ ]{11})", RegexOptions.IgnoreCase);
            var match = regex.Match(url);

            return match.Success ? match.Groups[1].Value : null;
        }

        private async Task AnalyzeVideo()
        {
            requestOngoing = true;

            await HttpClientService.PostToAPIAsync(EndpointConstants.MarketResearchEndpoint + $"/{MarketResearch.Id}/video", new YouTubeVideoAnalysisDTO { Url = VideoProposal.Url });

            requestOngoing = false;

            await CloseModalAsync();
        }
    }
}
