using NuGet.Protocol.Core.Types;
using Shared.ChatMessages;
using System;

namespace Server.Features.MarketResearches
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsSystem { get; set; }

        public ChatMessageDTO ToDTO()
        {
            return new ChatMessageDTO
            {
                IsSystem = IsSystem,
                Text = Text,
            };
        }

        public static ChatMessage FromDTO(ChatMessageDTO dto)
        {
            return new ChatMessage
            {
                IsSystem = dto.IsSystem,
                Text = dto.Text,
            };
        }
    }
}
