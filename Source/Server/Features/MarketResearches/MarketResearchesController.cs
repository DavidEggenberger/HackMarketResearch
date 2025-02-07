using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure.EFCore;
using System.Collections.Generic;

namespace Server.Features.MarketResearches
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketResearchesController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;

        public MarketResearchesController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IEnumerable<string> GetMarketResearches()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
