using Blazored.Modal.Services;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;
using Client.BuildingBlocks.Inputs.Emoji;
using Client.BuildingBlocks.Modals;

namespace Client.BuildingBlocks.Inputs
{
    public partial class NameWithEmojiInput : InputBase<string>
    {
        [Inject]
        public IModalService ModalService { get; set; }

        [Parameter, EditorRequired]
        public Expression<Func<string>> ValidationFor { get; set; } = default!;

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public string PlaceHolder { get; set; }

        [Parameter]
        public string Tooltip { get; set; }

        [Parameter]
        public EventCallback<string> EmojiSelectedCallback { get; set; }

        private string originalValue;
        private string selectedEmojii;
        private IModalReference modalReference;

        protected override async Task OnInitializedAsync()
        {
            originalValue = CurrentValue;
        }

        protected override bool TryParseValueFromString(string? value, out string result, out string validationErrorMessage)
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }

        private void RevertToOriginalValue()
        {
            CurrentValue = originalValue;
        }

        private void ResetEmoji()
        {
            selectedEmojii = string.Empty;
        }

        public async Task ShowEmojiModalAsync()
        {
            var parameters = new ModalParameters
            {
                { nameof(EmojiPickerModal.ModalExitedCallback), new EventCallback<string>(this, async (string chosenEmoji) => { await SelectEmojiAsync(chosenEmoji); modalReference.Close(); }) },
            };

            modalReference = ModalService.Show<EmojiPickerModal>(string.Empty, parameters, DefaultModalOptions.DefaultModal);
        }

        private async Task SelectEmojiAsync(string selectedEmoji)
        {
            selectedEmojii = selectedEmoji;
            await EmojiSelectedCallback.InvokeAsync(selectedEmoji);
        }
    }
}
