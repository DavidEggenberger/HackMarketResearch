using Client.BuildingBlocks.Http;
using Microsoft.AspNetCore.Components;

namespace Client.BuildingBlocks.Modals
{
    public partial class VideoAnalysisModal : ComponentBase
    {
        [Parameter]
        public EventCallback ModalExitedCallback { get; set; }

        [Inject]
        public HttpClientService HttpClientService { get; set; }

        public async Task CloseModalAsync()
        {
            if (ModalExitedCallback.HasDelegate)
            {
                await ModalExitedCallback.InvokeAsync(this);
            }
        }
    }
}
