using Shared.ChatMessages;
using System;

namespace Server.Features.MarketResearches
{
    public class MarketProposal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public MarketProposalDTO ToDTO()
        {
            return new MarketProposalDTO { Id = Id, Name = Name };
        }
    }
}
