using NuGet.Protocol.Core.Types;
using Shared.ChatMessages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Features.MarketResearches
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsSystem { get; set; }
        public List<VideoProposal> VideoProposals { get; set; } = new List<VideoProposal>();
        public List<MarketProposal> MarketProposals { get; set; } = new List<MarketProposal>();

        public ChatMessageDTO ToDTO()
        {
            return new ChatMessageDTO
            {
                IsSystem = IsSystem,
                Text = Text,
                MarketProposals = MarketProposals.Select(m => m.ToDTO()).ToList(),
                VideoProposals = VideoProposals.Select(v => v.ToDTO()).ToList()
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
