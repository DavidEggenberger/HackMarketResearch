using Blazored.Modal.Services;
using Blazored.Modal;
using Client.BuildingBlocks.Http;
using Client.BuildingBlocks.Modals;
using Microsoft.AspNetCore.Components;
using Shared;
using Shared.MarketResearch;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Pages
{
    public partial class MarketResearch : ComponentBase
    {
        [Inject]
        public HttpClientService HttpClientService { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Inject]
        public IModalService modalService { get; set; }

        [CascadingParameter]
        public HubConnection HubConnection { get; set; }

        private MarketResearchDTO marketResearch;
        private IModalReference modalReference;

        protected override async Task OnInitializedAsync()
        {
            marketResearch = await HttpClientService.GetFromAPIAsync<MarketResearchDTO>(EndpointConstants.MarketResearchEndpoint + $"/{Id}");

            HubConnection.On(NotificationConstants.UpdateMarketResearch, async () =>
            {
                marketResearch = await HttpClientService.GetFromAPIAsync<MarketResearchDTO>(EndpointConstants.MarketResearchEndpoint + $"/{Id}");
                StateHasChanged();     
            });
        }

        private async Task OpenCreateVideoAnalysisModalAsync()
        {
            var parameters = new ModalParameters
            {
                { nameof(CreateVideoAnalysisModal.ModalExitedCallback), new EventCallback(this, async () => { modalReference.Close(); })},
                { nameof(CreateVideoAnalysisModal.MarketResearchId), marketResearch.Id },
            };

            modalReference = modalService.Show<CreateVideoAnalysisModal>(string.Empty, parameters, DefaultModalOptions.DefaultModal);
        }
    }
}
