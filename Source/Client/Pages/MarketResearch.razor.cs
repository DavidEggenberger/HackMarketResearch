using Blazored.Modal.Services;
using Blazored.Modal;
using Client.BuildingBlocks.Http;
using Client.BuildingBlocks.Modals;
using Microsoft.AspNetCore.Components;
using Shared;
using Shared.MarketResearch;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.YouTube;

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

        private List<YouTubeVideoAnalysisDTO> sortedVideos;

        protected override async Task OnInitializedAsync()
        {
            marketResearch = await HttpClientService.GetFromAPIAsync<MarketResearchDTO>(EndpointConstants.MarketResearchEndpoint + $"/{Id}");

            HubConnection.On(NotificationConstants.UpdateMarketResearch, async () =>
            {
                marketResearch = await HttpClientService.GetFromAPIAsync<MarketResearchDTO>(EndpointConstants.MarketResearchEndpoint + $"/{Id}");
                StateHasChanged();     
            });


            HubConnection.On(NotificationConstants.RefreshVideos, async () =>
            {
                marketResearch.Videos = await HttpClientService.GetFromAPIAsync<List<YouTubeVideoAnalysisDTO>>(EndpointConstants.MarketResearchEndpoint + $"/{Id}/videos");
                StateHasChanged();
            });

            sortedVideos = marketResearch.Videos;
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

        private async Task OpenSignInModalAsync()
        {
            var parameters = new ModalParameters
            {
                { nameof(SignInModal.ModalExitedCallback), new EventCallback(this, async () => { modalReference.Close(); })},
            };

            modalReference = modalService.Show<SignInModal>(string.Empty, parameters, DefaultModalOptions.DefaultModal);
        }

        private void FilterItems(ChangeEventArgs e)
        {
            var sorter = e.Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(sorter))
            {
                sortedVideos = marketResearch.Videos;
            }
            else
            {
                if (sorter == "Neuste")
                {
                    Console.WriteLine(90);
                }
            }
        }
    }
}
