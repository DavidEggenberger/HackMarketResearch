﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.EFCore;
using Server.Infrastructure.Hubs;
using Shared;
using Shared.ChatMessages;
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
        private readonly IHubContext<NotificationHub> hubContext;
        private readonly ApplicationDbContext applicationDbContext;

        public MarketResearchesController(ApplicationDbContext applicationDbContext, IHubContext<NotificationHub> hubContext)
        {
            this.applicationDbContext = applicationDbContext;
            this.hubContext = hubContext;
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
                .Include(mr => mr.VideoAnalysises)
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

        [HttpPost("{marketResearchId}/chat")]
        public async Task CreateChatMessageAsync([FromRoute] Guid marketResearchId, [FromBody] ChatMessageDTO chatMessageDTO)
        {
            var marketResearch = await applicationDbContext.MarketResearches.FirstAsync(mr => mr.Id == marketResearchId);

            marketResearch.ChatMessages.Add(ChatMessage.FromDTO(chatMessageDTO));

            await hubContext.Clients.All.SendAsync(NotificationConstants.UpdateChat);

            await applicationDbContext.SaveChangesAsync();
        }

        [HttpGet("{marketResearchId}/chat")]
        public async Task<List<ChatMessageDTO>> LoadAllChatMessageOfMarketResearchAsync([FromRoute] Guid marketResearchId)
        {
            var marketResearch = await applicationDbContext.MarketResearches
                .Include(mr => mr.ChatMessages)
                .FirstAsync(mr => mr.Id == marketResearchId);

            return marketResearch.ToDTO().ChatMessages;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
