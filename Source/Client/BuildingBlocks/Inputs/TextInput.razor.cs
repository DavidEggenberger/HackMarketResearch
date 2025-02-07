using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace Client.BuildingBlocks.Inputs
{
    public partial class TextInput : InputBase<string>
    {
        [Parameter, EditorRequired]
        public Expression<Func<string>> ValidationFor { get; set; } = default!;

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public string PlaceHolder { get; set; }

        [Parameter]
        public string Tooltip { get; set; }

        private string originalValue;

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
    }
}
