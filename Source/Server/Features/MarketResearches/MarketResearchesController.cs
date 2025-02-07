using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.EFCore;
using Shared.MarketResearch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult> GetMarketResearch(Guid id)
        {
            var marketResearch = await applicationDbContext.MarketResearches.FirstAsync(mr => mr.Id == id);

            return Ok(marketResearch.ToDTO());
        }

        [HttpPost]
        public async Task<ActionResult> CreateMarketResearch([FromBody] MarketResearchDTO marketResearchDTO)
        {
            await Task.Delay(1000);

            var marketResearch = MarketResearch.FromDTO(marketResearchDTO);
            marketResearch.Id = Guid.NewGuid();

            applicationDbContext.MarketResearches.Add(marketResearch);
            await applicationDbContext.SaveChangesAsync();

            return Ok(marketResearch.ToDTO());
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
