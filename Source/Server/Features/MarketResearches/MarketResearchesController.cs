﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Features.YouTube;
using Server.Infrastructure.EFCore;
using Server.Infrastructure.Hubs;
using Server.Infrastructure.SemanticKernel;
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
        private readonly VideoAnalyzer videoAnalyzer;
        private readonly SemanticKernelService semanticKernelService;

        public MarketResearchesController(ApplicationDbContext applicationDbContext, VideoAnalyzer videoAnalyzer, IHubContext<NotificationHub> hubContext, SemanticKernelService semanticKernelService)
        {
            this.applicationDbContext = applicationDbContext;
            this.hubContext = hubContext;
            this.videoAnalyzer = videoAnalyzer;
            this.semanticKernelService = semanticKernelService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMarketResearch(Guid id)
        {
            var marketResearch = await applicationDbContext.MarketResearches
                .Include(mr => mr.VideoAnalysises)
                .ThenInclude(va => va.Comments)
                .Include(mr => mr.ChatMessages)
                .ThenInclude(va => va.VideoProposals)
                .Include(mr => mr.ChatMessages)
                .ThenInclude(c => c.MarketProposals)
                .FirstAsync(mr => mr.Id == id);

            return Ok(marketResearch.ToDTO());
        }

        [HttpGet("VideosToBeAnalyzed")]
        public async Task<ActionResult> GetVideosToBeAnalyzed()
        {
            var videoAnalyzations = await applicationDbContext
                .YouTubeVideoAnalyses
                .Include(mr => mr.MarketResearch)
                .Include(c => c.Comments)
                .Where(x => string.IsNullOrEmpty(x.AnalyzationResult) && x.Comments.Any() && !string.IsNullOrEmpty(x.MarketResearch.ProductDescription))
                .Select(x => new VideoWaitingForAnalysisDTO { ProductDescription = x.MarketResearch.ProductDescription, Id = x.Id, VideoComments = x.Comments.Select(c => c.ToDTO()).ToList() })
                .ToListAsync();

            return Ok(videoAnalyzations);
        }

        [HttpPost("{marketResearchId}/YouTubeSearch")]
        public async Task GenerateYouTubeSearch([FromRoute] Guid marketResearchId, [FromBody] MarketProposalDTO marketProposalDTO)
        {
            var marketResearch = await applicationDbContext.MarketResearches.FirstAsync(mr => mr.Id == marketResearchId);

            var foundVideos = await videoAnalyzer.SearchYouTubeVideos(marketProposalDTO.Name);

            marketResearch.ChatMessages.Add(new ChatMessage
            {
                IsSystem = true,
                Text = "Hier habe ich Video vorschläge für dich",
                VideoProposals = foundVideos.Select(fv => new VideoProposal { Url = fv.Url, Title = fv.VideoName }).ToList()
            });

            await applicationDbContext.SaveChangesAsync();

            await hubContext.Clients.All.SendAsync(NotificationConstants.UpdateMarketResearch);
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
                    Text = "Willkommen! Unser Ziel ist es Nischenmärkte für dein Produkt/Idee zu finden. Was hast du für eine ProduktIdee?"
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

            var youtubeId = GetYouTubeVideoId(youTubeVideoAnalysisDTO.Url);
            var (videoName, thumbnail) = await videoAnalyzer.GetYouTubeVideoTitle(youtubeId);
            var comments = await videoAnalyzer.AnalyzeYouTubeVideo(youtubeId);

            marketResearch.VideoAnalysises.Add(new YouTube.YouTubeVideoAnalysis
            {
                VideoName = videoName,
                Url = youTubeVideoAnalysisDTO.Url,
                Thumbnail = thumbnail,
                Comments = comments
            });

            await applicationDbContext.SaveChangesAsync();

            await hubContext.Clients.All.SendAsync(NotificationConstants.RefreshVideos);
        }

        [HttpPost("{marketResearchId}/chat")]
        public async Task CreateChatMessageAsync([FromRoute] Guid marketResearchId, [FromBody] ChatMessageDTO chatMessageDTO)
        {
            var marketResearch = await applicationDbContext.MarketResearches.FirstAsync(mr => mr.Id == marketResearchId);

            marketResearch.ChatMessages.Add(ChatMessage.FromDTO(chatMessageDTO));

            var chatMessages = await semanticKernelService.Chat(marketResearch, chatMessageDTO.Text);

            marketResearch.ChatMessages.AddRange(chatMessages);

            await applicationDbContext.SaveChangesAsync();

            await hubContext.Clients.All.SendAsync(NotificationConstants.UpdateChat);
        }

        [HttpGet("{marketResearchId}/chat")]
        public async Task<List<ChatMessageDTO>> LoadAllChatMessageOfMarketResearchAsync([FromRoute] Guid marketResearchId)
        {
            var marketResearch = await applicationDbContext.MarketResearches
                .Include(mr => mr.ChatMessages)
                .ThenInclude(mr => mr.MarketProposals)
                .Include(mr => mr.ChatMessages)
                .ThenInclude(mr => mr.VideoProposals)
                .FirstAsync(mr => mr.Id == marketResearchId);

            return marketResearch.ToDTO().ChatMessages;
        }

        [HttpGet("{marketResearchId}/videos")]
        public async Task<List<YouTubeVideoAnalysisDTO>> LoadAllVideosOfMarketResearchAsync([FromRoute] Guid marketResearchId)
        {
            var marketResearch = await applicationDbContext.MarketResearches
                .Include(mr => mr.VideoAnalysises)
                .ThenInclude(va => va.Comments)
                .FirstAsync(mr => mr.Id == marketResearchId);

            return marketResearch.VideoAnalysises.Select(v => v.ToDTO()).ToList();
        }

        [HttpPut("{videoId}")]
        public async Task UpdateVideoAnalyzation(Guid videoId, [FromBody] UpdateVideoAnalysisDTO updateVideo)
        {
            var videoAnalysis = await applicationDbContext.YouTubeVideoAnalyses.FirstAsync(ytva => ytva.Id == videoId);

            videoAnalysis.AnalyzationResult = updateVideo.Result;

            await applicationDbContext.SaveChangesAsync();

            await hubContext.Clients.All.SendAsync(NotificationConstants.RefreshVideos);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private string GetYouTubeVideoId(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            // Regular expression to extract YouTube video ID
            var regex = new Regex(@"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^""&?\/ ]{11})", RegexOptions.IgnoreCase);
            var match = regex.Match(url);

            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
