using Microsoft.AspNetCore.Components;
using Shared.ChatMessages;
using Shared.MarketResearch;

namespace Client.Components
{
    public partial class ChatComponent : ComponentBase
    {
        [Parameter]
        public MarketResearchDTO MarketResearchDTO { get; set; }

        protected override async Task OnInitializedAsync()
        {
            
        }
    }
}
