using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.BuildingBlocks.Modals
{
    public partial class CreateModal<TModel> : ComponentBase
    {
        [Parameter]
        public TModel Model { get; set; }

        [Parameter]
        public string CreationTitle { get; set; }

        [Parameter]
        public string CreationDescription { get; set; }

        [Parameter]
        public RenderFragment Template { get; set; }

        [Parameter]
        public EventCallback ModalExitedCallback { get; set; }

        [Parameter]
        public EventCallback ActionButtonClickedCallback { get; set; }

        [Parameter]
        public string IconPath { get; set; }

        private EditContext context;
        private bool ActionButtonEnabled;
        private string ActionButtonStyling => ActionButtonEnabled ?
            "cursor-pointer border-primary-500 bg-primary-500 border fill-white p-1 text-white duration-200 hover:border-1 hover:border-primary-500 hover:fill-primary-500 hover:text-primary-500 hover:bg-white-700" :
            "ring-1 cursor-default rounded p-1 shadow-sm ring-inset ring-gray-300 hover:bg-gray-50";
        private bool requestOngoing;

        protected override async Task OnInitializedAsync()
        {
            context = new EditContext(Model);
            context.OnValidationStateChanged += (st, o) =>
            {
                if (context.GetValidationMessages().Any() is false)
                {
                    ActionButtonEnabled = true;
                }
                else
                {
                    ActionButtonEnabled = false;
                }
                StateHasChanged();
            };

            if (context.Validate())
            {
                ActionButtonEnabled = false;
            }
        }

        private async Task CloseModalAsync()
        {
            if (ModalExitedCallback.HasDelegate)
            {
                await ModalExitedCallback.InvokeAsync(this);
            }
        }

        private async Task CreateButtonClickedAsync()
        {
            requestOngoing = true;

            if (ActionButtonClickedCallback.HasDelegate)
            {
                await ActionButtonClickedCallback.InvokeAsync(this);
            }

            requestOngoing = false;

            await CloseModalAsync();
        }
    }
}
