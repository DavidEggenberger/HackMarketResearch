using Server.Features.UserIdentity;
using Server.Features.YouTube;
using Shared.MarketResearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Server.Features.MarketResearches
{
    public class MarketResearch
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public List<YouTubeVideoAnalysis> VideoAnalysises { get; set; } = new List<YouTubeVideoAnalysis>();
        public List<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public List<MarketProposal> PotentialMarkets { get; set; }

        public string Emoji { get; set; }
        public string Name { get; set; }
        public MarketType MarketType { get; set; }
        public string ProductDescription { get; set; }

        public static MarketResearch FromDTO(MarketResearchDTO marketResearch)
        {
            return new MarketResearch
            {
                Emoji = marketResearch.Emoji,
                Name = marketResearch.Name,
            };
        }

        public MarketResearchDTO ToDTO()
        {
            return new MarketResearchDTO
            {
                Id = Id,
                Emoji = Emoji,
                Name = Name,
                ChatMessages = ChatMessages.Select(cm => cm.ToDTO()).ToList(),
                Videos = VideoAnalysises.Select(v =>  v.ToDTO()).ToList(),
            };
        }
    }
}
