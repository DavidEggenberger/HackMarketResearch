using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ChatMessages
{
    public class ChatMessageDTO
    {
        public bool IsSystem { get; set; }
        public string Text { get; set; }
        public List<VideoProposalDTO> VideoProposals { get; set; }
        public List<MarketProposalDTO> MarketProposals { get; set; }
    }
}
