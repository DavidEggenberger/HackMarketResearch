using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.EFCore;
using Shared.MarketResearch;
using Shared.YouTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var marketResearch = await applicationDbContext.MarketResearches
                .Include(mr => mr.ChatMessages)
                .FirstAsync(mr => mr.Id == id);

            return Ok(marketResearch.ToDTO());
        }

        [HttpGet("VideosToBeAnalyzed")]
        public async Task<ActionResult> GetVideosToBeAnalyzed()
        {
            var videoAnalyzations = await applicationDbContext.YouTubeVideoAnalyses
                .Where(yva => yva.Comments.Count == 0)
                .Select(w => new VideoWaitingForAnalysisDTO { Id = w.Id, YoutubeId = w.Url }).ToListAsync();

            string GetYouTubeVideoId(string url)
            {
                if (string.IsNullOrWhiteSpace(url))
                    return null;

                // Regular expression to extract YouTube video ID
                var regex = new Regex(@"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^""&?\/ ]{11})", RegexOptions.IgnoreCase);
                var match = regex.Match(url);

                return match.Success ? match.Groups[1].Value : null;
            }

            return Ok(videoAnalyzations.Select(v => new VideoWaitingForAnalysisDTO { Id = v.Id, YoutubeId = GetYouTubeVideoId(v.YoutubeId) }));
        }

        [HttpPost]
        public async Task<ActionResult> CreateMarketResearch([FromBody] MarketResearchDTO marketResearchDTO)
        {
            await Task.Delay(1000);

            var marketResearch = MarketResearch.FromDTO(marketResearchDTO);
            marketResearch.Id = Guid.NewGuid();
            marketResearch.ChatMessages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    IsSystem = true,
                    Text = "Willkommen bei der Marktanalyse!"
                }
            };

            applicationDbContext.MarketResearches.Add(marketResearch);
            await applicationDbContext.SaveChangesAsync();

            return Ok(marketResearch.ToDTO());
        }

        [HttpPost("{marketResearchId}/video")]
        public async Task CreateYouTubeVideoAnalysis([FromRoute] Guid marketResearchId, [FromBody] YouTubeVideoAnalysisDTO youTubeVideoAnalysisDTO)
        {
            var marketResearch = await applicationDbContext.MarketResearches.FirstAsync(mr => mr.Id == marketResearchId);

            marketResearch.VideoAnalysises.Add(new YouTube.YouTubeVideoAnalysis
            {
                Url = youTubeVideoAnalysisDTO.Url,
            });

            await applicationDbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
