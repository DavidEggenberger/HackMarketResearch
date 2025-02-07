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
        public HttpClientService HttpClientService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private MarketResearchDTO marketResearch = new MarketResearchDTO();

        private async Task CreateMarketResearchAsync()
        {
            var createdMarketResearch = await HttpClientService.PostToAPIAsync<MarketResearchDTO>(EndpointConstants.MarketResearchEndpoint, marketResearch);

            await CloseModalAsync();

            NavigationManager.NavigateTo($"/marketResearch/{createdMarketResearch.Id}");
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
