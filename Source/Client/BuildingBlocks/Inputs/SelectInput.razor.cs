using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Client.BuildingBlocks.Inputs
{
    public partial class SelectInput<T> : InputBase<T>
    {
        [Parameter, EditorRequired]
        public Expression<Func<T>> ValidationFor { get; set; } = default!;

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public string PlaceHolder { get; set; }

        [Parameter]
        public string Tooltip { get; set; }

        [Parameter]
        public List<(T, string)> Options { get; set; }

        private T originalValue;

        protected override async Task OnInitializedAsync()
        {
            originalValue = CurrentValue;
        }

        protected override bool TryParseValueFromString(string? value, out T result, out string validationErrorMessage)
        {
            Console.WriteLine(value);
            result = default!;
            validationErrorMessage = null;
            return true;
        }

        private void RevertToOriginalValue()
        {
            CurrentValue = originalValue;
        }
    }
}
