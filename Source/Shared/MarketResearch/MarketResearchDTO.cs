using Shared.ChatMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.MarketResearch
{
    public class MarketResearchDTO
    {
        public Guid Id { get; set; }
        public string Emoji { get; set; }
        public string Name { get; set; }
        public MarketTypeDTO MarketType { get; set; }
        public List<ChatMessageDTO> ChatMessages { get; set; }
    }
}
