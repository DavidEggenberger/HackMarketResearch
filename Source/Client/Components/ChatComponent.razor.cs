using Client.BuildingBlocks.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Shared;
using Shared.ChatMessages;
using Shared.MarketResearch;

namespace Client.Components
{
    public partial class ChatComponent : ComponentBase
    {
        [CascadingParameter]
        public HubConnection HubConnection { get; set; }

        [Inject]
        public HttpClientService HttpClientService { get; set; }

        [Parameter]
        public MarketResearchDTO MarketResearchDTO { get; set; }

        private string currentMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            HubConnection.On(NotificationConstants.UpdateChat, async () => await GetMessagesForChat());
        }

        private async Task SendChatMessageAsync(string message)
        {
            var chatMessage = new ChatMessageDTO { IsSystem = false, Text = message };

            await HttpClientService.PostToAPIAsync<ChatMessageDTO>(EndpointConstants.MarketResearchEndpoint + $"/{MarketResearchDTO.Id}/chat", chatMessage);

            MarketResearchDTO.ChatMessages.Add(chatMessage);

            currentMessage = string.Empty;
        }

        private async Task HandleKeyDown(KeyboardEventArgs args)
        {
            if (args.Key == "Enter" && currentMessage?.Length > 1)
            {
                await SendChatMessageAsync(currentMessage);
            }
        }

        private async Task GetMessagesForChat()
        {
            var messages = await HttpClientService.GetFromAPIAsync<List<ChatMessageDTO>>(EndpointConstants.MarketResearchEndpoint + $"/{MarketResearchDTO.Id}/chat");
            MarketResearchDTO.ChatMessages = messages;
            StateHasChanged();
        }
    }
}
