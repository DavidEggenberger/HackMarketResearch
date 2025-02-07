using Shared.ChatMessages;
using System;

namespace Server.Features.MarketResearches
{
    public class VideoProposal
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public VideoProposalDTO ToDTO()
        {
            return new VideoProposalDTO
            {
                Description = Description,
                Title = Title,
                Url = Url
            };
        }
    }
}
