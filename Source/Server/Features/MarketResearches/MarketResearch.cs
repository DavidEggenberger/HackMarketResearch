using Server.Features.UserIdentity;
using Shared.MarketResearch;
using System;
using System.Reflection.Metadata.Ecma335;

namespace Server.Features.MarketResearches
{
    public class MarketResearch
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string Emoji { get; set; }
        public string Name { get; set; }
        public MarketType MarketType { get; set; }

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
            };
        }
    }
}
