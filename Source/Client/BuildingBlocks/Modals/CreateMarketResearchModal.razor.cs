using Client.BuildingBlocks.Http;
using Microsoft.AspNetCore.Components;
using Shared;
using Shared.MarketResearch;

namespace Client.BuildingBlocks.Modals
{
    public partial class CreateMarketResearchModal : ComponentBase
    {
        [Parameter]
        public EventCallback ModalExitedCallback { get; set; }

        [Inject]
        public AuthorizedHttpClientService HttpClientService { get; set; }

        private MarketResearchDTO marketResearch = new MarketResearchDTO();

        private async Task CreateMarketResearchAsync()
        {
            await HttpClientService.PostToAPIAsync(EndpointConstants.MarketResearchEndpoint, marketResearch);

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
