using Microsoft.AspNetCore.Components;

namespace Client.BuildingBlocks.Modals
{
    public partial class SignInModal : ComponentBase
    {
        [Parameter]
        public EventCallback ModalExitedCallback { get; set; }

        [Parameter]
        public NavigationManager NavigationManager { get; set; }

        public async Task CloseModalAsync()
        {
            if (ModalExitedCallback.HasDelegate)
            {
                await ModalExitedCallback.InvokeAsync(this);
            }
        }
    }
}
