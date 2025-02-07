﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class EndpointConstants
    {
        public const string LoginPath = "/UserIdentity/Login";
        public const string LogoutPath = "/UserIdentity/Logout";
        public const string UserClaimsPath = "/UserIdentity/BFFUser";

        public const string MarketResearchEndpoint = "/marketresearches";
    }
}
