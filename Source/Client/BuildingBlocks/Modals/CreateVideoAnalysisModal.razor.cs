using Client.BuildingBlocks.Http;
using Client.Pages;
using Microsoft.AspNetCore.Components;
using Shared.MarketResearch;
using Shared;
using Shared.YouTube;

namespace Client.BuildingBlocks.Modals
{
    public partial class CreateVideoAnalysisModal : ComponentBase
    {
        [Parameter]
        public EventCallback ModalExitedCallback { get; set; }

        [Inject]
        public HttpClientService HttpClientService { get; set; }

        [Parameter]
        public Guid MarketResearchId { get; set; }

        private YouTubeVideoAnalysisDTO videoAnalysisDTO;

        protected override async Task OnInitializedAsync()
        {
            videoAnalysisDTO = new YouTubeVideoAnalysisDTO();


        }

        private async Task CreateVideoAnalysisAsync()
        {
            await HttpClientService.PostToAPIAsync<YouTubeVideoAnalysisDTO>(EndpointConstants.MarketResearchEndpoint + $"/{MarketResearchId}/video", videoAnalysisDTO);

            await CloseModalAsync();
        }

        public async Task CloseModalAsync()
        {
            if (ModalExitedCallback.HasDelegate)
            {
                await ModalExitedCallback.InvokeAsync(this);
            }
        }
    }
}
