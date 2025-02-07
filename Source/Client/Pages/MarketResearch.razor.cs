using Client.BuildingBlocks.Http;
using Microsoft.AspNetCore.Components;
using Shared;
using Shared.MarketResearch;

namespace Client.Pages
{
    public partial class MarketResearch : ComponentBase
    {
        [Inject]
        public HttpClientService HttpClientService { get; set; }

        [Parameter]
        public string Id { get; set; }

        private MarketResearchDTO marketResearch;

        protected override async Task OnInitializedAsync()
        {
            marketResearch = await HttpClientService.GetFromAPIAsync<MarketResearchDTO>(EndpointConstants.MarketResearchEndpoint + $"/{Id}");
        }
    }
}
