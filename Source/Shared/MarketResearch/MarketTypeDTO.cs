using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.MarketResearch
{
    public enum MarketTypeDTO
    {
        HealthCare,
        Finance,
        Education,
        Robotics
    }

    public static class MarketTypeDTOExtensions
    {
        public static string ToString(this MarketTypeDTO marketTypeDTO)
        {
            return marketTypeDTO switch
            {
                MarketTypeDTO.HealthCare => "⚕️ Healthcare",
                MarketTypeDTO.Education => "📖 Education",
                MarketTypeDTO.Robotics => "🦾 Robotics",
                MarketTypeDTO.Finance => "📈 Finance"
            };
        }
    }
}
