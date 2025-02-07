using Microsoft.AspNetCore.Identity;
using Server.Features.MarketResearches;
using System.Collections.Generic;

namespace Server.Features.UserIdentity
{
    public class ApplicationUser : IdentityUser
    {
        public List<MarketResearch> MarketResearches { get; set; }
    }
}
