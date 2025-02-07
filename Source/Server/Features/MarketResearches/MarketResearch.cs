using Server.Features.UserIdentity;
using Shared.MarketResearch;

namespace Server.Features.MarketResearches
{
    public class MarketResearch
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string Emoji { get; set; }
        public string Name { get; set; }





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
                Emoji = Emoji,
                Name = Name,
            };
        }
    }
}
