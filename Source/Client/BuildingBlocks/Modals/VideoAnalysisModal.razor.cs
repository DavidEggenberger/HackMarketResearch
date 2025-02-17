﻿using Client.BuildingBlocks.Http;
using Microsoft.AspNetCore.Components;
using Shared.YouTube;

namespace Client.BuildingBlocks.Modals
{
    public partial class VideoAnalysisModal : ComponentBase
    {
        [Parameter]
        public EventCallback ModalExitedCallback { get; set; }

        [Parameter]
        public YouTubeVideoAnalysisDTO VideoAnalysis { get; set; }

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
