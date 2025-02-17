﻿using Server.Features.MarketResearches;
using Shared.YouTube;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Features.YouTube
{
    public class YouTubeVideoAnalysis
    {
        public Guid Id { get; set; }
        public MarketResearch MarketResearch { get; set; }
        public string VideoName { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public string AnalyzationResult { get; set; }
        public YouTubeVideoAnalysisState State { get; set; }
        public List<VideoComment> Comments { get; set; }

        public YouTubeVideoAnalysisDTO ToDTO()
        {
            return new YouTubeVideoAnalysisDTO
            {
                VideoName = VideoName,
                Url = Url,
                Thumbnail = Thumbnail,
                AnalyzationResult = AnalyzationResult,
                VideoComments = Comments.Select(c => new VideoCommentDTO { LikeCount = c.LikeCount, Text = c.Text }).ToList()
            };
        }
    }
}
